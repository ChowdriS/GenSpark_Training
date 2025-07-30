import { Component, OnInit, signal } from '@angular/core';
import { NewsService } from '../../services/news-service';
import { News } from '../../model/Models';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-news-list',
  imports: [RouterLink,CommonModule],
  templateUrl: './news-list.html',
  styleUrl: './news-list.css'
})
export class NewsList implements OnInit{
  news = signal<News[]>([]);
  ngOnInit(): void {
    this.getAllNews();
  }
  constructor(private newsService: NewsService) {
  }
  getAllNews(){
    this.newsService.getAllNews().subscribe({
      next:(res:any)=>{
        this.news.set(res.$values);
      },
      error:(err:any)=>{
        console.log(err);
      }
    })
  }
}
