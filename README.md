Easynvest.Infohub.Parse

Introduced Cache in InfoHub API

In this project was implemented a new layer of cache using Redis. Redis is a cache based in key-value store and very fast, I've created this implementation using to a very famous design patterns known as "Decorator Pattern", was very simple do introduce in codebase.

To run project you need to run these commands in root folder:

docker build . -t api-info-hub
docker run api-info-hub
docker run -d -p 6379:6379 -i -t redis:3.2.5-alpine
