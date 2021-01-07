import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ImportExportService } from '../shared/import-export.service';

@Component({
  selector: 'app-import',
  templateUrl: './import.component.html',
  styleUrls: ['./import.component.scss']
})
export class ImportComponent implements OnInit {
  treeName: string;
  file: File;

  constructor(
    private service: ImportExportService,
    private router: Router
  ) { }

  ngOnInit(): void {
  }

  uploadFile(files: File[]) {
    this.file = files[0];
  }

  onSubmit() {
    const formData: FormData = new FormData();
    if (this.treeName) {
      formData.append('treeName', this.treeName);
    }
    if(this.file) {
      formData.append('file', this.file);
    }

    this.service.createTreeFromFile(formData).subscribe(res => {
      if(res) {
        this.router.navigate(['']);
      }
    });
  }

}
