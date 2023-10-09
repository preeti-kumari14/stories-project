import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { StoryDetails } from '../models/story-details';

@Injectable({
  providedIn: 'root'
})
export class StoriesService {

  storiesApiURL = 'http://localhost:5000/api/Stories';

  constructor(private http: HttpClient) { }

  getStoriesDetails(): Observable<StoryDetails[]> {
    return this.http.get<StoryDetails[]>(this.storiesApiURL) as Observable<StoryDetails[]>;
  }
}
