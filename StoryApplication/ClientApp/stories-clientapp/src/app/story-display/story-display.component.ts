import { Component, ViewChild } from '@angular/core';
import { StoryDetails } from 'src/models/story-details';
import { StoriesService } from 'src/services/stories-service.service';

import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-story-display',
  templateUrl: './story-display.component.html',
  styleUrls: ['./story-display.component.css']
})
export class StoryDisplayComponent {

  resultStories: StoryDetails[] = [];
  dataloading = false;
  pageLength = 1;
  pageSize = 10;
  displayedColumns: string[] = [
    'storyId',
    'storyTitle',
    'storyUrl'
  ];
  
  @ViewChild(MatPaginator, { static: false })
  set paginator(value: MatPaginator) {
    if (this.dataSource) {
      this.dataSource.paginator = value;
    }
  }

  @ViewChild(MatSort, { static: false })
  set sort(value: MatSort) {
    if (this.dataSource) {
      this.dataSource.sort = value;
    }
  }
  dataSource = new MatTableDataSource<StoryDetails>();
  constructor(private storiesService : StoriesService){ }
  ngOnInit() {
    this.dataloading = true;
    this.getStudyDetails();
  }

  getStudyDetails() {
    this.storiesService.getStoriesDetails().subscribe(
      (study) => {
        console.log(study);
        this.resultStories = [];
        this.resultStories = study;
        this.dataSource = new MatTableDataSource(this.resultStories);
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
        this.pageLength = this.resultStories.length;
        if (this.pageLength < 10) {
          this.pageSize = this.pageLength;
        }
        this.dataloading = false;
      },
      (err) => {
        this.resultStories = [];
        this.dataloading = false;
        console.error(err);
      }
    );

  }

}
