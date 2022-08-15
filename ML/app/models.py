from pyexpat import model
from shutil import move
import turtle
from django.db import models
from django.contrib.auth.models import User

# Create your models here.


class Movie(models.Model):
    title = models.CharField(max_length=100)
    budget = models.BigIntegerField(blank=True, null=True)
    poster = models.CharField(max_length=150, null=True)
    trailer = models.CharField(max_length=150, null=True)
    genres = models.TextField(blank=True, null=True)
    description = models.TextField(blank=True, null=True)
    favorite = models.ManyToManyField(User, related_name='favorite')

    def get_budget_in_yen(self):
        return self.budget * 122

    keywords = models.TextField(blank=True, null=True)
    overview = models.TextField(blank=True, null=True)
    tagline = models.TextField(blank=True, null=True)
    cast = models.TextField(blank=True, null=True)
    director = models.TextField(blank=True, null=True)


class News(models.Model):
    title = models.CharField(max_length=255)


class Genre(models.Model):
    name = models.CharField(max_length=100)
    description = models.TextField(blank=True, null=True)


class Review(models.Model):
    review = models.CharField(max_length=200)
    created_at = models.DateTimeField(auto_now=True)
    movie = models.ForeignKey(
        Movie, on_delete=models.CASCADE, related_name='movie')
    user = models.ForeignKey(
        User, on_delete=models.CASCADE, related_name='user')
