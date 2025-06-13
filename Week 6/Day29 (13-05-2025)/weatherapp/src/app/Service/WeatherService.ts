import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, interval, switchMap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class WeatherService {
  private citySubject = new BehaviorSubject<string>('');
  city$ = this.citySubject.asObservable();
  private apiKey = 'a27eca224b7bd4635f63b5eb69e60b44';

  constructor(private http: HttpClient) {}

  setCity(city: string) {
    this.citySubject.next(city);
  }

  getWeather(city: string) {
    const url = `https://api.openweathermap.org/data/2.5/weather?q=${city}&appid=${this.apiKey}&units=metric`;
    return this.http.get(url);
  }
}