# FAQ Chatbot API with FastAPI & Sentence Transformers

 - Implemented a simple chatbot-style FAQ engine built using **FastAPI** and [`all-MiniLM-L6-v2`] model from **Sentence Transformers**,a transformer-based model 
    optimized for **semantic textual similarity** and **sentence embeddings**.
 - It reads a list of FAQs from a JSON file and returns the best matching answer for a user’s query using semantic similarity
 - Serve via a FastAPI backend with two endpoints:
  - `GET /faqs` — List all FAQ items
  - `POST /query` — Get the most relevant answer for a user query