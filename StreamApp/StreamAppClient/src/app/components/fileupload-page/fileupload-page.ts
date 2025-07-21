import { Component } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { VideoService } from '../../services/video-service';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-fileupload-page',
  imports: [ReactiveFormsModule,RouterLink],
  templateUrl: './fileupload-page.html',
  styleUrl: './fileupload-page.css'
})
export class FileuploadPage {
  videoForm: FormGroup;

  constructor(private fb: FormBuilder, private videoService : VideoService, public router : Router) {
    this.videoForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      videoFile: [null, Validators.required]
    });
  }

  onFileChange(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.videoForm.patchValue({ videoFile: file });
      this.videoForm.get('videoFile')?.updateValueAndValidity();
    }
  }

  onSubmit() {
    const formData = new FormData();
    formData.append('title', this.videoForm.get('title')?.value);
    formData.append('description', this.videoForm.get('description')?.value);
    formData.append('videoFile', this.videoForm.get('videoFile')?.value);

    this.videoService.uploadVideo(formData).subscribe({
      next:()=>{
        this.router.navigate(['../home']);
      },
      error:()=>{
        alert("something went wrong!")
      }
    });
  }
}
