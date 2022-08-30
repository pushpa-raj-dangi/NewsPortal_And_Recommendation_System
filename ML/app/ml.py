
import pandas as pd
from sklearn.feature_extraction.text import CountVectorizer
import requests

from sklearn.metrics.pairwise import cosine_similarity

from .models import Movie


def combined_feature(row):
    return row['genres'] + '' + row['keywords'] + '' + '' + row['cast']


def get_id_from_index(df, index):

    return df[df.index == index]["id"].values[0]


def get_index_from_id(df, id):
    return df[df.id == id]["id"].values[0]


def get_index_from_id(df, id):
    return df[df.id == id].index.values[0]


def get_recommendation_for_movie(movie_id):

    df = pd.DataFrame(list(Movie.objects.all().values()))
    features = ['keywords', 'cast', 'genres', 'director', 'overview']
    for feature in features:
        df[feature] = df[feature].fillna('')

    df["combined_features"] = df.apply(combined_feature, axis=1)
    cv = CountVectorizer()
    count_matrix = cv.fit_transform(df["combined_features"])
    cosine_sim = cosine_similarity(count_matrix)

    id = get_index_from_id(df, movie_id)

    similar_news = list(enumerate(cosine_sim[id]))
    sorted_similar_news = sorted(
        similar_news, key=lambda x: x[1], reverse=True)
    print(sorted_similar_news)


    i = 0
    movie_ids = []
    for movie in sorted_similar_news[1:]:
        i = i+1
        movie_ids.append(get_id_from_index(df, movie[0]))

        if i > 15:
            break
    return movie_ids
