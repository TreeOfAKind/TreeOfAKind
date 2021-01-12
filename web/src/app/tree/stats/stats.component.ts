import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TreeStats } from '../shared/tree-stats.model';
import { TreeService } from '../shared/tree.service';
import { ApexChart, ApexNonAxisChartSeries, ApexResponsive } from 'ng-apexcharts';

@Component({
  selector: 'app-stats',
  templateUrl: './stats.component.html',
  styleUrls: ['./stats.component.scss']
})
export class StatsComponent implements OnInit {
  emptyOptions: ChartOptions = {
    series: null,
    chart: null,
    responsive: null,
    labels: null
  };
  genderChartOptions: ChartOptions = this.emptyOptions;
  livingChartOptions: ChartOptions = this.emptyOptions;
  marriedChartOptions: ChartOptions = this.emptyOptions;
  treeId: string;
  model: TreeStats = {
    totalNumberOfPeople: null,
    averageLifespanInDays: null,
    numberOfPeopleOfEachGender: null,
    numberOfLivingPeople: null,
    numberOfDeceasedPeople: null,
    averageNumberOfChildren: null,
    numberOfMarriedPeople: null,
    numberOfSinglePeople: null
  };

  chartSettings: ApexChart = {
    width: 380,
    type: "pie"
  };
  responsiveSettings: ApexResponsive[] = [
    {
      breakpoint: 480,
      options: {
        chart: { width: 300 },
        legend: { position: "bottom" }
      }
    }
  ];

  constructor(
    private service: TreeService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.treeId = this.route.parent.snapshot.paramMap.get('id');
    this.service.getTreeStatistics(this.treeId).subscribe(result => {
      this.model = result;
      this.prepareGenderChart(result);
      this.prepareLivingChart(result);
      this.prepareMarriedChart(result);
    });
  }

  private prepareGenderChart(stats: TreeStats) {
    this.genderChartOptions = this.prepareChartOptions(
      [
        stats.numberOfPeopleOfEachGender.Female,
        stats.numberOfPeopleOfEachGender.Male,
        stats.numberOfPeopleOfEachGender.Other,
        stats.numberOfPeopleOfEachGender.Unknown,
      ],
      [ 'Female', 'Male', 'Other', 'Unknown' ]
    )
  }

  private prepareLivingChart(stats: TreeStats) {
    this.livingChartOptions = this.prepareChartOptions(
      [
        stats.numberOfLivingPeople,
        stats.numberOfDeceasedPeople
      ],
      [ 'Living', 'Deceased' ]
    )
  }

  private prepareMarriedChart(stats: TreeStats) {
    this.marriedChartOptions = this.prepareChartOptions(
      [
        stats.numberOfMarriedPeople,
        stats.numberOfSinglePeople
      ],
      [ 'Married', 'Single' ]
    )
  }

  private prepareChartOptions(series: number[], labels: string[]): ChartOptions {
    const options: ChartOptions = {
      series: series,
      chart: this.chartSettings,
      labels: labels,
      responsive: this.responsiveSettings
    };
    return options;
  }

}

export interface ChartOptions {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  responsive: ApexResponsive[];
  labels: any;
};
