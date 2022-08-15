
import json
import pandas as pd
import math
from multiprocessing import context
from urllib import request, response
from django.http import HttpResponse
from django.shortcuts import redirect, render

from app.news import get_recommendation_for_news

import requests
from rest_framework.views import APIView
from rest_framework.response import Response
from rest_framework import status


# Create your views here.

def get_index(request):
    return HttpResponse("index")


class GetSinglePost(APIView):
    def get(self, request, id):
        movie = r = requests.get(
            f'https://localhost:5001/api/posts/{id}', verify=False)

        return Response(movie.json(), status=status.HTTP_200_OK)


class GetSingleNews(APIView):
    def get(self, request, id):
        movie_ids = get_recommendation_for_news(id)
        print("*********************** this is ids")
        ids = ""
        for val in enumerate(movie_ids):
            ids += f"&id={val[1]}"
        print(ids)
        recomendations = requests.get(
            "https://localhost:5001/api/posts/recommendations?"+ids, verify=False)

        return Response(recomendations.json(), status=status.HTTP_200_OK)


class GetNewsRecommendation(APIView):
    def get(self, request, id):
        movie_ids = get_recommendation_for_news(id)
        print(movie_ids, "ids")
        response = requests.get(
            'https://localhost:5001/api/posts', verify=False)
        return Response(response.json(), status=status.HTTP_200_OK)
