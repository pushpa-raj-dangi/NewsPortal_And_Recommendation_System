from django.test import TestCase
from django.contrib.auth.models import User

from .models import Movie


class MovieTestCase(TestCase):
    def setup(self):
        movie = Movie.objects.create(title='Batman', budget=1000)
        user1 = User.objects.create(
            username='john1', email='john@doe.com', password='test@123')

        user2 = User.objects.create(
            username='john2', email='john@doe.com', password='test@123')

        movie.favorite.add(user1)
        movie.favorite.add(user2)

    def test_movie_name(self):
        batman = Movie.objects.create(title='Batman')
        self.assertEqual(batman.title, 'Batman')

    def test_budget_in_yen(self):
        batman = Movie.objects.get(title='Batman')
        self.assertEqual(batman.get_budget_in_yen(), 1220000)
