from django.contrib import admin
from .models import Genre, Movie, Review


class MovieAdmin(admin.ModelAdmin):
    list_display = ('title', 'budget', 'genres', 'keywords', 'tagline')


class GenreAdmin(admin.ModelAdmin):
    list_display = ('name', 'description')


class ReviewAdmin(admin.ModelAdmin):
    list_display = ('review')


# Register your models here.
admin.site.register(Movie, MovieAdmin)
admin.site.register(Genre, GenreAdmin)
admin.site.register(Review)
