
# import pandas as pd
# from sklearn.feature_extraction.text import CountVectorizer

# from sklearn.metrics.pairwise import cosine_similarity


# def combined_feature(row):
#     return row['genres'] + "" + row["keywords"] + "" + row["tagline"] + "" + row["cast"]


# def get_id_from_index(df, index):
#     return df[df.index == index]["id"].values[0]


# def get_index_from_id(df, id):
#     return df[df.id == id]["id"].values[0]


# def get_index_from_id(df, id):
#     return df[df.id == id].index.values[0]


# def get_recommendation_for_movie(movie_id):
#     id = get_index_from_id(df, movie_id)

#     similar_moives = list(enumerate(cosine_sim[id]))
#     sorted_similar_movies = sorted(
#         similar_moives, key=lambda x: x[1], reverse=True)

#     i = 0
#     movie_ids = []
#     for movie in sorted_similar_movies:
#         i = i+1
#         movie_ids.append(get_id_from_index(df, movie[0]))

#         if i > 15:
#             break
#     return movie_ids


# df = pd.DataFrame(list(Movie.objects.all().values()))
# features = ['keywords', 'cast', 'genres', 'director', 'overview']
# for feature in features:
#     df[feature] = df[feature].fillna('')

# df["combined_features"] = df.apply(combined_feature, axis=1)
# cv = CountVectorizer()
# count_matrix = cv.fit_transform(df["combined_features"])
# cosine_sim = cosine_similarity(count_matrix)
