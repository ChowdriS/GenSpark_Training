import { HttpClient } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { url } from '../misc/constants';
import { Video, VideoUploadPayload } from '../models/Video';

@Injectable({
  providedIn: 'root'
})
export class VideoService {

  constructor(private http:HttpClient){}

  getVideos():Observable<Video[]>{
    return this.http.get<Video[]>(`${url}`);
  }

  uploadVideo(payload:any){
    return this.http.post(`${url}/upload`,payload);
  }
}
