# Generated by Django 4.0.2 on 2022-02-21 01:11

from django.conf import settings
from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        migrations.swappable_dependency(settings.AUTH_USER_MODEL),
        ('app', '0003_genre'),
    ]

    operations = [
        migrations.AddField(
            model_name='movie',
            name='favorite',
            field=models.ManyToManyField(related_name='favorite', to=settings.AUTH_USER_MODEL),
        ),
    ]