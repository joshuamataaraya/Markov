import { Component, Inject, SkipSelf } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html',
  styles: ['div span {  display: inline-block;width: 18px;text-align: center; } .panel-red {border: 1px solid #f00;} .panel-not-red{border: 1px solid}']
})
export class FetchDataComponent {
  private http;
  private baseUrl;
  public cypher;
  public words;
  public base;
  public values;
  public puzzle;
  public wordsBreakdown;


  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
  }
  onClickMe() {
    var self = this;
    const headers = new HttpHeaders().set('content-type', 'multipart/form-data');
    var body = { cypher: this.cypher
    , values: this.values, "words": this.words, "rules": this.base }
    this.http.post(this.baseUrl + 'api/puzzle/Markov', body, headers
    ).subscribe(result => {
      // this.forecasts = result;
      console.log('Printing Results:')
      console.log(result)

      let ObjectsPuzzle = new Array();
      result.puzzle.forEach(row => {
        var objectList = new Array();
        row.forEach(element => {
          objectList.push({letter: element,isWord:false}) 
        });
        ObjectsPuzzle.push(objectList);
      });
      console.log(ObjectsPuzzle);

      result.wordsBreakdown.forEach(word => {
        word.breakdown.forEach(element => {
          ObjectsPuzzle[element.row][element.column].isWord = true;
          ObjectsPuzzle[element.row][element.column].letter = ObjectsPuzzle[element.row][element.column].letter.toLowerCase();
        });
      });
      self.puzzle = ObjectsPuzzle;
      self.wordsBreakdown = JSON.stringify(result.wordsBreakdown);
    }, error => console.error(error));
  }
  onImport(event, type) {
    var file = event.srcElement.files[0];
    if (file) {
      var reader = new FileReader();
      var self = this;
      reader.readAsText(file, "UTF-8");
      reader.onload = function (evt: Event & { target: { result: string } }) {
        // console.log(JSON.parse(evt.target.result));
        console.log(type)
        switch (type) {
          case "cypher":
            self.cypher = JSON.parse(evt.target.result);
            break;
          case "base":
            self.base = JSON.parse(evt.target.result);
            break;
          case "values":
            self.values = JSON.parse(evt.target.result);
            break;
          case "words":
            self.words = JSON.parse(evt.target.result);
            break;
        }
      }
    }
    reader.onerror = function (evt) {
      console.log('error reading file');
    }
  }
}