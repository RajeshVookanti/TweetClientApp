import { Component, OnInit } from '@angular/core';
import { take } from 'rxjs';
import { TwitterMetricsDto } from '../models/twitterMetricsDto';
import { TweetMetricsService } from '../services/tweet-metrics-service'
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Twitter Client ';
  public apiData: TwitterMetricsDto = new TwitterMetricsDto();
  constructor(private metricsService:TweetMetricsService){
  }
  
  ngOnInit(): void {
    this.loaddata();

  }

  refresh(){
    this.loaddata();
  }

  loaddata(){
    this.metricsService.getMetrics().pipe(take(1)).subscribe( data => {
      this.apiData = data;

    },err => { console.log(err) })
  }

}
