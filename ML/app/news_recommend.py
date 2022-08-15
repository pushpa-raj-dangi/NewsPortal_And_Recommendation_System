from audioop import reverse
from cmath import log
from dataclasses import field
from email import header
import gzip
from collections import defaultdict
import json
import random

# f = gzip.open("../amazon_reviews_us_Musical_Instruments_v1_00.tsv.gz",
f = json.load("https://localhost:5001/api/posts")
print(f)
header = f.readline()
header = header.strip().split('\t')

dataset = []

for line in f:
    fields = line.strip().split('\t')
    d = dict(zip(header, field))
    d['star_rating'] = int(d['star_rating'])
    d['helpful_votes'] = int(d['helpful_vote'])
    d['total_votes'] = int(d['total_votes'])

    dataset.append(d)


dataset[0]

users_per_item = defaultdict(set)
items_per_user = defaultdict(set)

item_names = {}

for d in dataset:
    user, item = d['customer_id'], d['product_id']
    users_per_item[item].add(user)
    item_names[item] = d['product_title']


def jaccard(s1, s2):
    numer = len(s1.inersection(s2))
    denom = len(s1.union(s2))
    return numer / denom


def mostSimilar(i):
    similarities = []
    users = users_per_item[i]
    for i2 in users_per_item:
        if i2 == i:
            continue
        sim = jaccard(users, users_per_item)
        similarities.append(sim, i2)
    similarities.sort(reverse=True)
    return similarities[:10]


dataset[2]

query = dataset[2]['product_id']


mostSimilar(query)

item_names[query]

[item_names[x[1]] for x in mostSimilar(query)]


def mostSimilarFast(i):
    similarities = []
    users = users_per_item[i]
    candidates_items = set()

    for u in users:
        candidates_items = candidates_items.union(items_per_user[u])

    for i2 in candidates_items:
        if i2 == i:
            continue
        sim = jaccard(users, users_per_item[i2])
    similarities.sort(reverse=True)
    return similarities[:10]
