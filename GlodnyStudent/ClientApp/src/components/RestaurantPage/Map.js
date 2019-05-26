import React, { Component } from 'react'

export default class Map extends Component {

    constructor(props){
        super(props);
        this.state={
            street:"",
            streetNumber:"",
            localNumber:"",
            district:"Bemowo",
            disabledSubmit:true,
            streetValidResult:false,
            streetNumberValidResult:false,
            localNumberValidResult:false,
            streetNumberErrorMessage:""
        }
       /*  this.handleInputChange = this.handleInputChange.bind(this); */
        this.handleSubmit = this.handleSubmit.bind(this);
        this.inputValidate = this.inputValidate.bind(this);
    }

   /*  handleInputChange(event) {
        const target = event.target;
        const value = target.value;
        const name = target.name;
    
        this.setState({
          [name]: value
        });

      } */

      handleSubmit(e){    
        const {street,streetNumber,localNumber,district} = this.state;
        this.props.updateAddress("address",( street + " " + streetNumber + " " + localNumber + " " + district));
        e.preventDefault();
    }



    inputValidate(event){
      const value = event.target.value;
      const name = event.target.name;
      const nameValid = name + "ValidResult";
      const {streetNumberValidResult,streetValidResult,localNumberValidResult} = this.state;
      let validResult =true;
      let eventValid = false;
      const regNumbersAndOneLetterAtEnd = /\d+[a-zA-Z]?$/;
      const regEmp = /^$/;
      let errorMessage = "";  

      if(regEmp.test(String(event.target.value))){
        validResult =false;
      }else{
        switch(event.target.name) {
          case "street":
              eventValid= (event.target.value.length > 0)?true:false;   
               validResult = eventValid && streetNumberValidResult && localNumberValidResult;
            break;
          case "streetNumber":
              eventValid = regNumbersAndOneLetterAtEnd.test(String(event.target.value));
              validResult = eventValid && streetValidResult && localNumberValidResult;
              errorMessage  = (eventValid  )?"":"Numer ulicy musi składać się z ciągu samych cyfr, ewentulalnie zakończonego jedną literą";            
            break;
          case "localNumber":
              eventValid = value;
              validResult = eventValid && streetNumberValidResult && streetValidResult;           
            break;
          case "district":
              eventValid= true;
              validResult = localNumberValidResult && streetNumberValidResult && streetValidResult;           
            break;       
          default:
        }
      }
     

      this.setState({
        disabledSubmit: !validResult,
        [name]: value,
        [nameValid]:eventValid,
        streetNumberErrorMessage:errorMessage
      });

    }


    render() {
        const districts = ["Bemowo","Białołęka","Bielany","Mokotów","Ochota","Praga-Południe","Praga-Północ","Rembertów","Śródmieście",
                           "Targówek","Ursus","Ursynów","Wawer","Wesoła","Wilanów","Włochy","Wola","Żoliborz"];
        const districtsList = districts.map(district=><option key={district} value={district}>{district}</option>);
        return (
            <div>
              <div className="map"></div>
              <div className="mapInfo">
                  <h3 className="wow fadeIn" data-wow-duration="2s">Znajdź nas na mapie!</h3>                  
                  <address className="wow fadeIn" data-wow-duration="2s">
                    <i className="fas fa-map-marker-alt fa-2x"></i> {this.props.address}
                  </address>
                  <form onSubmit={this.handleSubmit} id="addressInfo">
                      <label>Ulica <input type="text" name="street" onChange={this.inputValidate} /></label>
                      <label>Numer ulicy <input type="text" name="streetNumber" onChange={this.inputValidate} />{this.state.streetNumberErrorMessage}</label>
                      <label>Numer lokalu <input type="number" min="0" name="localNumber" onChange={this.inputValidate} /></label>
                      <label>Dzielnica 
                        <select name="district" onChange={this.inputValidate}>
                          {districtsList}
                        </select>
                      </label>
                      <input type="submit" disabled={this.state.disabledSubmit} value="Zapisz"/>
                  </form>
                  <button className="wow fadeIn" data-wow-duration="2s">Zobacz na mapach google</button>
              </div>
            </div>
        )
    }
}
