import React, { Component } from 'react';
import Map from 'pigeon-maps';
import Marker from 'pigeon-marker';
import Geocode from "react-geocode";
 
// set Google Maps Geocoding API for purposes of quota management. Its optional but recommended.
Geocode.setApiKey("AIzaSyBxvJXLoj0DtoGczKojLEo_Kc3LsdlPxCQ ");
 
// Enable or disable logs. Its optional.
Geocode.enableDebug();

export default class MapSection extends Component {

    constructor(props){
        super(props);
        this.state={
            streetName:"",
            streetNumber:"",
            localNumber:"",
            district:"Bemowo",
            disabledSubmit:true,
            streetNameValidResult:false,
            streetNumberValidResult:false,
            localNumberValidResult:false,
            streetNumberErrorMessage:"",
            zoom: 17,
            locationY:null,
            locationX:null,
         

            streetNameErrorMessage:"",
            localNumberErrorMessage:"",
            restaurantNameErrorMessage:"",
        }
        
       /*  this.handleInputChange = this.handleInputChange.bind(this); */
        this.inputValidate = this.inputValidate.bind(this);
        this.makeAddresObject = this.makeAddresObject.bind(this);
        this.getCoords = this.getCoords.bind(this);
    }


    getCoords(streetName,streetNumber){
        const address =`${streetName} ${streetNumber}`
        Geocode.fromAddress(address).then(
            response => {
                console.log(response.results[0].geometry.location);
                this.setState({
     
                    locationY: response.results[0].geometry.location.lat,
                    locationX: response.results[0].geometry.location.lng,        
                    mapResonseMessage:null
                });
            },
            error => {
                console.error(error);
                this.setState({
                    mapResonseMessage:"Ulica nie została znaleziona."
                }); 
            }
        );
    }



    zoomIn = () => {
      this.setState({
        zoom: Math.min(this.state.zoom + 1, 18)
      })
    }
  
    zoomOut = () => {
      this.setState({
        zoom: Math.max(this.state.zoom - 1, 1)
      })
    }

   /*  handleInputChange(event) {
        const target = event.target;
        const value = target.value;
        const name = target.name;
    
        this.setState({
          [name]: value
        });

      } */

    
      makeAddresObject(){
        const {streetName,streetNumber,localNumber,district,locationY,locationX} = this.state;
       return {streetName,streetNumber,localNumber,district,locationY,locationX};
      }


    inputValidate(event){
      const value = event.target.value;
      const name = event.target.name;
      const nameValid = name + "ValidResult";
      const errMsg = name + "ErrorMessage";
      const {streetNumberValidResult,streetNameValidResult,localNumberValidResult,restaurantNameValidResult} = this.state;
      let validResult =true;
      let eventValid = false;
      const regNumbersAndOneLetterAtEnd = /^\d+[a-zA-Z]?$/;
      const regEmp = /^$/;
      let errorMessage = "";  

      if(regEmp.test(String(event.target.value))){
        validResult =false;
      }else{
        switch(event.target.name) {
           case "restaurantName":
                eventValid= (event.target.value.length > 2)?true:false;   
               validResult = eventValid && streetNumberValidResult && localNumberValidResult && streetNameValidResult;
            break;
         /*  case "cuisine":
              eventValid= true;
              validResult = localNumberValidResult && streetNumberValidResult && streetNameValidResult && restaurantNameValidResult;
            break;  */  
          case "streetName":
              this.getCoords(event.target.value,this.state.streetNumber);  
              eventValid= this.state.mapResonseMessage?false:true;
               validResult = eventValid && streetNumberValidResult && localNumberValidResult && restaurantNameValidResult;;
            break;
          case "streetNumber":
              this.getCoords(this.state.streetName,event.target.value);
              let crdres = this.state.mapResonseMessage?false:true;
              eventValid = regNumbersAndOneLetterAtEnd.test(String(event.target.value)) && crdres;
              validResult = eventValid && streetNameValidResult && localNumberValidResult && restaurantNameValidResult;
              errorMessage  = (regNumbersAndOneLetterAtEnd.test(String(event.target.value)) )?null:"Numer ulicy musi składać się z ciągu samych cyfr, ewentulalnie zakończonego jedną literą";           
            break;
          case "localNumber":
              eventValid= (event.target.value.length > 0)?true:false;
              validResult = eventValid && streetNumberValidResult && streetNameValidResult && restaurantNameValidResult;           
            break;
          /* case "district":
              eventValid= true;
              validResult = localNumberValidResult && streetNumberValidResult && streetNameValidResult && restaurantNameValidResult;           
            break;  */      
          default:
        }
      }
     // dopisac wersje z krs

      this.setState({
        disabledSubmit: !validResult,
        [name]: value,
        [nameValid]:eventValid,
        [errMsg]:errorMessage
      });

    }

    render() {
      const {streetName,streetNumber,localNumber,district} = this.props.address;
        const districts = ["Bemowo","Białołęka","Bielany","Mokotów","Ochota","Praga-Południe","Praga-Północ","Rembertów","Śródmieście",
                           "Targówek","Ursus","Ursynów","Wawer","Wesoła","Wilanów","Włochy","Wola","Żoliborz"];
        const districtsList = districts.map(district=><option key={district} value={district}>{district}</option>);



        const editInputs = this.props.ownerLogIn&&sessionStorage.getItem("id")?
        <form onSubmit={(e)=>this.props.updateAddress(e,this.makeAddresObject(),`${this.props.restaurantId}/UpdateAddress`)} id="addressInfo">
          {this.state.streetNameErrorMessage}
          {this.state.localNumberErrorMessage}
          {this.state.restaurantNameErrorMessage}
        <div>
            <label>Ulica
              <input id="streetName" type="text" name="streetName" onChange={this.inputValidate} />
            </label> 
        </div>
        <div>
            <label>Numer ulicy
              <input id="streetNumber" type="text" name="streetNumber" onChange={this.inputValidate} />
            </label>
            <p>{this.state.streetNumberErrorMessage}</p>
        </div>
        <div>
            <label>Numer lokalu
              <input id="localNumber" type="number" min="0" name="localNumber" onChange={this.inputValidate} />
            </label>
        </div>
        <div>
            <label>Dzielnica
              <select id="district" name="district" onChange={this.inputValidate}>
                {districtsList}
              </select>
            </label>
        </div>
      <input type="submit" disabled={this.state.disabledSubmit} value="Zapisz"/>
  </form>:"";





        return (
            <div>
              <div>
                  <div>
                  <Map center={[this.props.lat,this.props.lng]} zoom={this.state.zoom}>
                    <Marker anchor={[this.props.lat,this.props.lng]} payload={1} onClick={({ event, anchor, payload }) => {}}/>
                  </Map>
                  </div>

                  <button onClick={this.zoomIn}><i ></i></button> {/* className="fas fa-search-plus" */}
                  <button onClick={this.zoomOut}><i ></i></button> {/* className="fas fa-search-minus" */}
              </div>
              <div>
                  <h3>Znajdź nas na mapie!</h3>                  
                  <address>
                    <i ></i> {streetName} {streetNumber}/{localNumber} {district} {/* className="fas fa-map-marker-alt fa-2x" */}
                  </address>
                    {editInputs}
                  <button>Zobacz na mapach google</button>
              </div>
            </div>
        )
    }
}