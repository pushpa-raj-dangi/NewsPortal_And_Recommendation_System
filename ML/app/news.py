

from .models import Movie
from sklearn.metrics.pairwise import cosine_similarity
import requests
from sklearn.feature_extraction.text import CountVectorizer
import pandas as pd
import urllib3
urllib3.disable_warnings(urllib3.exceptions.InsecureRequestWarning)

def combined_feature(row):
    return row['tag'] + " " + row['category'] + " " + row['name']



def get_id_from_index(df, index):
    return df[df.index == index]["id"].values[0]


def get_index_from_id(df, id):
    return df[df.id == id]["id"].values[0]


def get_index_from_id(df, id):
    return df[df.id == id].index.values[0]


def get_recommendation_for_news(news_id):
    print(f"&&&&&&&&&&&&&&&&&&&&&&&0930039330303 __________________ {news_id}")
    response = requests.get('https://localhost:5001/api/posts', verify=False)
    posts = response.json()
    df = pd.DataFrame(posts)
    features = ['name','tag','category']
    for feature in features:
        df[feature] = df[feature].fillna('')

    df["combined_features"] = df.apply(combined_feature, axis=1)
    cv = CountVectorizer()
    count_matrix = cv.fit_transform(df["combined_features"])
    cosine_sim = cosine_similarity(count_matrix)

    id = get_index_from_id(df, news_id)

    similar_moives = list(enumerate(cosine_sim[id]))
    sorted_similar_news = sorted(
        similar_moives, key=lambda x: x[1], reverse=True)
    print(sorted_similar_news)
    i = 0
    news_ids = []
    for news in sorted_similar_news[1:]:
        i = i+1
        news_ids.append(get_id_from_index(df, news[0]))

        if i > 5:
            break
    return news_ids
