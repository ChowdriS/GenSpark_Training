import { Component, OnInit } from '@angular/core';
import { News } from '../../model/Models';
import { NewsService } from '../../services/news-service';
import { FormsModule, NgModel, ReactiveFormsModule } from '@angular/forms';
import { CommonModule, NgClass } from '@angular/common';

@Component({
  selector: 'app-news-mangement',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, FormsModule],
  templateUrl: './news-mangement.html',
  styleUrl: './news-mangement.css'
})
export class NewsMangement implements OnInit {
  newsList: News[] = [];
  isEditing = false;
  editingId: number | null = null;
  originalNews: Partial<News> | null = null;

  newsForm = {
    userId: 1,
    title: '',
    shortDescription: '',
    image: '',
    content: '',
    createdDate: new Date().toISOString().slice(0, 16),
    status: 1
  };

  constructor(private newsService: NewsService) {}

  ngOnInit() {
    this.loadNews();
  }

  loadNews() {
    this.newsService.getAll().subscribe({
      next: (data:any) => {
        this.newsList = data.$values;
      },
      error: (err:any) => {
        console.error('Failed to load news:', err);
      }
    });
  }

  onSubmit() {
    const submittedData = {
      ...this.newsForm,
      createdDate: new Date(this.newsForm.createdDate).toISOString()
    };

    if (this.isEditing && this.editingId !== null && this.originalNews) {
      const patch: any = {};
      for (const key in submittedData) {
        if (
          submittedData[key as keyof typeof submittedData] !==
          this.originalNews[key as keyof typeof submittedData]
        ) {
          patch[key] = submittedData[key as keyof typeof submittedData];
        }
      }

      if (Object.keys(patch).length === 0) {
        console.log('No changes to update.');
        return;
      }

      this.newsService.update(this.editingId, patch).subscribe({
        next: () => {
          this.resetForm();
          this.loadNews();
        },
        error: (err) => {
          console.error('Update failed:', err);
        }
      });
    } else {
      this.newsService.create(submittedData).subscribe({
        next: () => {
          this.resetForm();
          this.loadNews();
        },
        error: (err) => {
          console.error('Create failed:', err);
        }
      });
    }
  }

  editNews(news: News) {
    this.newsForm = {
      userId: news.userId!,
      title: news.title!,
      shortDescription: news.shortDescription!,
      image: news.image!,
      content: news.content!,
      createdDate: news.createdDate.slice(0, 16)!,
      status: news.status!
    };

    this.originalNews = { ...news };
    this.editingId = news.newsId;
    this.isEditing = true;
  }

  deleteNews(id: number) {
    if (confirm('Are you sure you want to delete this news?')) {
      this.newsService.delete(id).subscribe({
        next: () => this.loadNews(),
        error: (err) => console.error('Delete failed:', err)
      });
    }
  }

  resetForm() {
    this.newsForm = {
      userId: 1,
      title: '',
      shortDescription: '',
      image: '',
      content: '',
      createdDate: new Date().toISOString().slice(0, 16),
      status: 1
    };
    this.originalNews = null;
    this.isEditing = false;
    this.editingId = null;
  }
}
