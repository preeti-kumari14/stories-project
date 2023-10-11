import { Component, ViewChild } from '@angular/core';
import { StoryDetails } from 'src/models/story-details';
import { StoriesService } from 'src/services/stories-service.service';

import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { MatSelectChange } from '@angular/material/select';
import { StoryFilter } from '../../models/story-filter';


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
  defaultValue = "All";
  filterDictionary = new Map<string, string>();

  dataSourceFilters = new MatTableDataSource<StoryDetails>();
  constructor(private storiesService : StoriesService){ }
  ngOnInit() {
    this.dataloading = true;
    this.getStudyDetails();

    //this.dataSource.filterPredicate = function (record, filter) {
    //  return record.storyTitle.indexOf(filter) != -1;
    //}

    this.dataSourceFilters.filterPredicate = function (record, filter) {
      var map = new Map(JSON.parse(filter));
      let isMatch = false;
      for (let [key, value] of map) {
        isMatch = (value == "All") || (record[key as keyof StoryDetails] == value);
        if (!isMatch) return false;
      }
      return isMatch;
    }



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

  applyEmpFilter(ob: MatSelectChange, empfilter: StoryFilter) {

    this.filterDictionary.set(empfilter.name, ob.value);


    var jsonString = JSON.stringify(Array.from(this.filterDictionary.entries()));

    this.dataSourceFilters.filter = jsonString;
    //console.log(this.filterValues);
  }


  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

}
