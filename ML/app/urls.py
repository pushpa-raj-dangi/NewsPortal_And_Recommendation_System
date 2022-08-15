from os import remove
from unicodedata import name
from django.contrib import admin
from django.urls import include, path
from app import views

urlpatterns = [
    path('', views.get_index),
    path('api/news/<int:id>', views.GetSinglePost.as_view()),
    path('api/news/<int:id>', views.GetNewsRecommendation.as_view()),
    path('api/post/<int:id>', views.GetSingleNews.as_view()),
]
