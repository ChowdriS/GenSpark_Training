import { Component, OnInit, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { Auth } from '../../services/Auth/auth';
import { Getrole } from '../../misc/Token';
import { NgOptimizedImage } from '@angular/common';
import { Slider } from "../slider/slider";
import { EventService } from '../../services/Event/event.service';

@Component({
  selector: 'app-landing-page',
  imports: [RouterLink, Slider],
  templateUrl: './landing-page.html',
  styleUrl: './landing-page.css',
  standalone: true
})
export class LandingPage {
  images = signal<any | null>(null);
  currentIndex = signal<number>(0);
  intervalId: any;
  constructor(private authService : Auth,private router : Router, private eventsService: EventService){}
      ngOnInit() {
        if (this.authService.getToken()) {
          let token = this.authService.getToken();
          // console.log(token);
          let role = Getrole(token);
          if(role === 'User')
            this.router.navigate(['/user']);
          else if(role === 'Manager')
            this.router.navigate(['/manager']);
          else if(role === 'Admin')
            this.router.navigate(['/admin']);
        }
    }
    GetAllEventImages() {
    this.eventsService.getAllEventImages().subscribe({
      next:(res:any)=>{
        this.images.set(res.$values);
        console.log(this.images());
      },
      error:(err:any)=>{

      }
    })
  }
  startSlider() {
    this.intervalId = setInterval(() => {
      const count = this.images()?.length || 0;
      if (count > 0) {
        const next = ((this.currentIndex()) + 1) % count;
        this.currentIndex.set(next);
      }
    }, 3500); 
  }

  goToSlide(index: number) {
    this.currentIndex.set(index);
  }
}