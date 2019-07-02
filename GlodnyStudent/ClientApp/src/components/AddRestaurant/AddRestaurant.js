import React, { Component } from 'react'
import KRScheck from './KRScheck';
import PropTypes from 'prop-types';
import Geocode from "react-geocode";
 
// set Google Maps Geocoding API for purposes of quota management. Its optional but recommended.
Geocode.setApiKey("AIzaSyBxvJXLoj0DtoGczKojLEo_Kc3LsdlPxCQ ");
 
// Enable or disable logs. Its optional.
Geocode.enableDebug();

export default class AddRestaurant extends Component {

    constructor(props){
        super(props);
        this.state={
            mapResonseMessage:null,
            cuisines:[],//ma nie byc wszystkie,enumerator
            restaurant:{
                restaurantName:"",
                cuisine:"Angielska",//mam wiedziec co bedzie pierwsze
                streetName:"",
                streetNumber:"",
                localNumber:null,
                district:"Bemowo",
                lat: null,
                lng: null
            }, 
            wantToBeOwner:false,
            KRSValid:false,
            responseMessage:"",
            typingTimer:null,

            streetNameValidResult:false,
            streetNumberValidResult:false,
            localNumberValidResult:false,
            restaurantNameValidResult:false,

            streetNameErrorMessage:"",
            streetNumberErrorMessage:"",
            localNumberErrorMessage:"",
            restaurantNameErrorMessage:"",
            disabledSubmit:true,

            validResult:false

        }
        this.getCuisines = this.getCuisines.bind(this);
        this.handleInputChange = this.handleInputChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
        this.startCountdownToValidate = this.startCountdownToValidate.bind(this);
        this.inputValidate = this.inputValidate.bind(this);
        this.clearTheCountdownToValidate = this.clearTheCountdownToValidate.bind(this);
        this.getCoords = this.getCoords.bind(this);
        this.setKRSValidationResult = this.setKRSValidationResult.bind(this);
        this.toggleWantToBeOwner = this.toggleWantToBeOwner.bind(this);
        this.blockButtonKrsCheck = this.blockButtonKrsCheck.bind(this);
    }

    componentDidMount(){
       this.getCuisines();
    }


    getCuisines(){
        const address = `api/Restaurants/AllCuisines`;// Zeby zwracał wszystkei jakie mozna wpisac, enumerator na serwerze
        fetch(address).then((response) => {
          if (response.ok) {
            return response.json();
          } else {
            throw new Error('Cuisines not found');
          }
        })
        .then((result) => {
      
            this.setState({
            error:false,
            cuisines:result,
          });
        })
        .catch((error) => {
          console.log(error);
          
        });
      }


      handleInputChange(event) {
        const target = event.target;
        const value = target.value;
        const name = target.name;
    
      
            this.setState(prevState => ({
                restaurant: {
                    ...prevState.restaurant,
                    [name]: value
                }
              }));
        
       
      }

      setKRSValidationResult(value){
        this.setState({KRSValid: value});
      }


      handleSubmit(e){
          e.preventDefault();
          //console.log(RESTAURACJA: ${JSON.stringify(this.state.restaurant)} KRS: ${this.state.KRS});
          const adr =`api/Restaurants`; 
        
           fetch(adr, {
          method: 'PUT',
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization':'Bearer ' + sessionStorage.getItem("token")

          },
          body: JSON.stringify(
            {
                restaurantName: this.state.restaurant.restaurantName,
                cuisine:this.state.restaurant.cuisine,
                address:{
                  streetName:this.state.restaurant.streetName,
                  streetNumber:this.state.restaurant.streetNumber,
                  localNumber:this.state.restaurant.localNumber,
                  district:this.state.restaurant.district
                },
                userName:sessionStorage.getItem("username"),
                lat:this.state.restaurant.lat,
                lng:this.state.restaurant.lng,
                gotOwner:this.state.wantToBeOwner?this.state.KRSValid:false
              
            }
          )
        }).then(res => res.json())
        .then((data) => { 
     
          this.setState({
            responseMessage:data.status === 200?null:"Ojoj, coś poszło nie tak"

          }); 
          if(data.status === 200){
            this.context.router.history.push(`/Restauracja/${data.id}`)
          }
          
        
         } )
        .catch((err)=>console.log(err))  
      }


      static contextTypes = {
        router: PropTypes.object
      }



      async getCoords(streetName,streetNumber){
          const address =`${streetName} ${streetNumber}`
          let res=null;
        await Geocode.fromAddress(address).then(
          response => {
            console.log(response.results[0].geometry.location);
            this.setState(prevState => ({
              restaurant: {
                  ...prevState.restaurant,
                  lat: response.results[0].geometry.location.lat,
                  lng: response.results[0].geometry.location.lng,
              },
              mapResonseMessage:null
             }),
             ()=>res =true);
          },
          error => {
            console.error(error);
            this.setState({
              mapResonseMessage:"Ulica nie została znaleziona."
            },()=>res=false); 
          }
        );
        console.log(`res ${res}`);
        return res;
      }
//#############################################################
startCountdownToValidate(event){
  event.persist()
  clearTimeout(this.state.typingTimer);
  let Timer = setTimeout(this.inputValidate, 1000,event);
  this.setState({
    typingTimer:Timer
  });
}

clearTheCountdownToValidate(){
  clearTimeout(this.state.typingTimer);
}


async inputValidate(event){
  const value = event.target.value;
      const name = event.target.name;
      const nameValid = name + "ValidResult";
      const errMsg = name + "ErrorMessage";
      const {streetNumberValidResult,streetNameValidResult,localNumberValidResult,restaurantNameValidResult,wantToBeOwner,KRSValid} = this.state;
      let validResult =true;
      let eventValid = false;
      const regNumbersAndOneLetterAtEnd = /^\d{1,3}[a-zA-Z]?$/;
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
              
              
              eventValid= await this.getCoords(event.target.value,this.state.restaurant.streetNumber);
                console.log(`mapResponseMessage ${this.state.mapResonseMessage}
              eventValid ${eventValid}`);
               validResult = eventValid && streetNumberValidResult && localNumberValidResult && restaurantNameValidResult;
              
            break;
          case "streetNumber":
            
              eventValid = regNumbersAndOneLetterAtEnd.test(String(event.target.value));
              validResult = eventValid && streetNameValidResult && localNumberValidResult && restaurantNameValidResult;
              errorMessage  = (regNumbersAndOneLetterAtEnd.test(String(event.target.value)) )?null:"Numer ulicy musi składać się z ciągu maksymalnie 3 cyfr, ewentualnie zakończonego jedną literą";           
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

      console.log(`validResult: ${validResult}`);
      this.setState({
        disabledSubmit: !(validResult && (wantToBeOwner?KRSValid:true)),
        [name]: value,
        [nameValid]:eventValid,
        [errMsg]:errorMessage,
        validResult:validResult

      });

}


blockButtonKrsCheck(){
  this.setState({
    disabledSubmit: !(this.state.validResult && (this.state.wantToBeOwner?this.state.KRSValid:true))
  });
}



//#############################################################


toggleWantToBeOwner(){

    this.setState(prevState =>({
      wantToBeOwner: !prevState.wantToBeOwner
  }), () => {
    this.blockButtonKrsCheck();
  });
  
}


    render() {

        const cousinesList = this.state.cuisines.map(cusine=><option key={cusine} value={cusine}>{cusine}</option>);
        const districts = ["Bemowo","Białołęka","Bielany","Mokotów","Ochota","Praga-Południe","Praga-Północ","Rembertów","Śródmieście",
        "Targówek","Ursus","Ursynów","Wawer","Wesoła","Wilanów","Włochy","Wola","Żoliborz"];
        const districtsList = districts.map(district=><option key={district} value={district}>{district}</option>);
        

        return (
          <div>
            <div>
                <h2>Dodaj restauracje</h2>
            </div>
            <div >
              <div>
                <h3>{this.state.responseMessage}</h3>
                <h3>{this.state.mapResonseMessage}</h3>
                <form>
                  <div>
                    <label >Nazwa restauracji
                      <input  id="restaurantName" name="restaurantName" type="text" onChange={this.handleInputChange} onKeyUp={this.startCountdownToValidate} onKeyDown={this.clearTheCountdownToValidate} onBlur={this.inputValidate} />
                    </label>
                    <p>{this.state.restaurantNameErrorMessage}</p>
                  </div>
                  <div>
                    <label>Typ kuchni
                      <select id="cuisine" name="cuisine" onChange={this.handleInputChange} >
                          {cousinesList}
                      </select>
                    </label>
                  </div>
                  <div>
                    <label>Ulica
                      <input id="streetName" type="text" name="streetName" onKeyUp={this.startCountdownToValidate} onKeyDown={this.clearTheCountdownToValidate} onBlur={this.inputValidate} onChange={this.handleInputChange}/>
                    </label>
                    <p>{this.state.streetNameErrorMessage}</p>
                  </div>  
                  <div>
                    <label>Numer ulicy
                      <input id="streetNumber" type="text" name="streetNumber" onKeyUp={this.startCountdownToValidate} onKeyDown={this.clearTheCountdownToValidate} onBlur={this.inputValidate}  onChange={this.handleInputChange}/>
                    </label>
                    <p>{this.state.streetNumberErrorMessage}</p>
                  </div>  
                  <div>
                    <label>Numer lokalu
                      <input id="localNumber" type="number" min="0" name="localNumber"onKeyUp={this.startCountdownToValidate} onKeyDown={this.clearTheCountdownToValidate} onBlur={this.inputValidate} onChange={this.handleInputChange}/>
                    </label>
                    <p>{this.state.localNumberErrorMessage}</p>
                  </div>  
                  <div>
                    <input id="wantToBeOwner" type="checkbox"  name="wantToBeOwner" onClick={this.toggleWantToBeOwner} /> <label className="wantToBeOwnerLabel" for="wantToBeOwner">Chcę zostać włascicielem</label> 
                  </div>
                  <div>
                    <KRScheck blockButtonKrsCheck={this.blockButtonKrsCheck} wantToBeOwner={this.state.wantToBeOwner} setKRSValidationResult={this.setKRSValidationResult} />
                  </div>
                  <div>
                    <label>Dzielnica
                      <select name="distric" onChange={this.handleInputChange} >
                        {districtsList}
                      </select>
                    </label>
                  </div>
                  <div>
                    <button disabled={this.state.disabledSubmit} onClick ={this.handleSubmit}>Dodaj resturacje</button>
                  </div>
                </form>
              </div>
            </div>
          </div>
        )
    }
}