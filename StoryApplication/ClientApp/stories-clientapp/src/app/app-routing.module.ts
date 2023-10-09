import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoryDisplayComponent } from './story-display/story-display.component';


const routes: Routes = [
  { path: '', redirectTo: 'story-display', pathMatch: 'full' },
  { path: 'story-display', component: StoryDisplayComponent },
  { path: '**', component: StoryDisplayComponent }];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
