# Generated by Django 4.0.2 on 2022-02-23 02:20

from django.db import migrations, models


class Migration(migrations.Migration):

    dependencies = [
        ('app', '0006_movie_poster'),
    ]

    operations = [
        migrations.AddField(
            model_name='movie',
            name='trailer',
            field=models.CharField(max_length=150, null=True),
        ),
    ]