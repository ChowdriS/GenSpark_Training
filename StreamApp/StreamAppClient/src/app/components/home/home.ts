import { Component, OnInit, Signal, signal } from '@angular/core';
import { Video } from '../../models/Video';
import { VideoService } from '../../services/video-service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home implements OnInit {
  videos = signal<Video[]>([]);
  ngOnInit(): void {
    this.getVideos();
  }
  constructor(private videoService : VideoService){}
  getVideos(){
    this.videoService.getVideos().subscribe({
      next:(res:any)=>{
        this.videos.set(res.$values);
        console.log(this.videos())
      }
    })
  }
}
