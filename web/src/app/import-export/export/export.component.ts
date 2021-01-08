import { Component, Input, OnInit } from '@angular/core';
import { ImportExportService } from '../shared/import-export.service';

@Component({
  selector: 'app-export',
  templateUrl: './export.component.html',
  styleUrls: ['./export.component.scss']
})
export class ExportComponent implements OnInit {
  @Input() treeId: string;

  constructor(
    private service: ImportExportService
  ) { }

  ngOnInit(): void {
  }

  downloadFile() {
    this.service.getTreeFileExport(this.treeId).subscribe(result => {
      var blob = new Blob([result], { type: 'text/xml' });
      this.myCallback(blob);
    });
  }

  private myCallback(blob: Blob) {
    var url = window.URL.createObjectURL(blob);
    var filename = `tree_${this.treeId}.xml`;

    var a = document.createElement("a");
    // @ts-ignore
    a.style = "display: none";
    a.href = url;
    a.download = filename;

    // IE 11
    if (window.navigator.msSaveBlob !== undefined) {
      window.navigator.msSaveBlob(blob, filename);
      return;
    }

    document.body.appendChild(a);
    requestAnimationFrame(() => {
      a.click();
      window.URL.revokeObjectURL(url);
      document.body.removeChild(a);
    });
  }
}
