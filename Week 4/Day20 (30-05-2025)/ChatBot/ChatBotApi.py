from fastapi import FastAPI, HTTPException
from pydantic import BaseModel
from sentence_transformers import SentenceTransformer, util
import torch
from typing import List
import json
import uvicorn

app = FastAPI()

class FAQItem(BaseModel):
    question: str
    answer: str

class UserQuery(BaseModel):
    question: str

with open('Data/faq.json', 'r', encoding='utf-8') as f:
    faq_data = json.load(f)

device = 'cuda' if torch.cuda.is_available() else 'cpu'
model = SentenceTransformer('all-MiniLM-L6-v2', device=device)

questions = [faq['question'] for faq in faq_data]
question_embeddings = model.encode(questions, convert_to_tensor=True)

@app.get("/faqs", response_model=List[FAQItem])
def get_all_faqs():
    """Get all FAQ questions and answers"""
    return faq_data

@app.post("/query")
def get_answer(query: UserQuery):
    """Get the best matching answer for a user question"""
    try:
        input_embedding = model.encode(query.question, convert_to_tensor=True)
        scores = util.cos_sim(input_embedding, question_embeddings)
        best_idx = scores.argmax().item()
        
        return {
            "question": query.question,
            "matched_question": faq_data[best_idx]['question'],
            "answer": faq_data[best_idx]['answer'],
            "confidence": scores[0][best_idx].item()
        }
    except Exception as e:
        raise HTTPException(status_code=500, detail=str(e))

if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8000)