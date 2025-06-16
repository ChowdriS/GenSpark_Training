# Create a simple Angular application that lets users browse and search for products using infinite scroll and debounce-based search. The app will have basic routing with two routes: Home and About.
# Requirements:
# 1. Routing Setup
Implement basic routing with two routes:
/home → displays the product listing
/about → static page with dummy text (e.g., "This is a demo app built using Angular")
# 2. Product Listing Page (/home)
Fetch data from the DummyJSON API:
URL: https://dummyjson.com/products/search?q=<searchTerm>&limit=10&skip=<skip>
Show product cards with:
Product title
Thumbnail image
Price
# 3. Debounce Search
Add a search input with debounce (400ms) using RxJS.
On search, call the API with the query and reset pagination.
# 4. Infinite Scroll
Implement infinite scroll by listening to the scroll event.
Load more products when the user nears the bottom of the list.
Use the skip parameter for pagination.
# 5. Loading Indicator
Show a loader while fetching data.