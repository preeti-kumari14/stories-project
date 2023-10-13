import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map } from 'rxjs';
import { StoryDetails } from '../models/story-details';

@Injectable({
  providedIn: 'root'
})
export class StoriesService {

  storiesApiURL = 'http://localhost:5000/api/Stories';

  constructor(private http: HttpClient) { }

  getStoriesDetails(): Observable<StoryDetails[]> {
    return this.http.get<any>(this.storiesApiURL).pipe(
      map(response => {
        return response.stories as StoryDetails[];
      }),
    );
  }
}
