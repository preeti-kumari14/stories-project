import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StoryDisplayComponent } from './story-display.component';

describe('StoryDisplayComponent', () => {
  let component: StoryDisplayComponent;
  let fixture: ComponentFixture<StoryDisplayComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [StoryDisplayComponent]
    });
    fixture = TestBed.createComponent(StoryDisplayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
