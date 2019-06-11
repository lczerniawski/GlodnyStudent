import React, { Component } from 'react';
import './RestaurantPage.css';
import ReviewsCreator from './ReviewsCreator';
import PropTypes from 'prop-types';
import Gallery from './Gallery';
import Menu from './Menu';
import ReviewsList from './ReviewsList';
import HeaderImage from './HeaderImage';
import MapSection from './MapSection';
import RestaurantName from './RestaurantName';
import axios from 'axios';

export default class RestaurantPage extends Component {


constructor(props){
    super(props);
    this.state={
          responseMessageRemoveImage:null,
          responseMessageUploadImage:null,
          responseMessageMenuAddItem:null,
          responseMessageChangeAddress:null,
          responseMessageChangeName:null,
          responseMessageRemoveMenuItem:null,
          responseMessageReviews:null,
          responseMessage:null,
          fields: {},
          ownerLogIn:false,
          location:window.location.href.slice(window.location.href.lastIndexOf("/")+1),
          newReview:'',
          restaurant:{
            id:0,
            address:{streetName:"",streetNumber:"",localNumber:0,district:""},
            gallery:[],
            menu:[],
            name:"",
            reviews:[],
            ownerId:null,
            gotOwner:false,
            lat:null,
            lng:null
          }

    };
    this.handleInputChange = this.handleInputChange.bind(this);
    this.sendReview =this.sendReview.bind(this);
    this.backToRestaurationList = this.backToRestaurationList.bind(this);
    this.handleImageRemove = this.handleImageRemove.bind(this);
    this.handleRemoveMenuItem = this.handleRemoveMenuItem.bind(this);
    //this.SendRestaurantInfo = this.SendRestaurantInfo.bind(this);
    this.uploadJustFile = this.uploadJustFile.bind(this);
    this.filesOnChange = this.filesOnChange.bind(this);
    this.makeReport = this.makeReport.bind(this);
    this.removeRestaurant = this.removeRestaurant.bind(this);
    this.changeName = this.changeName.bind(this);
    this.changeAddress = this.changeAddress.bind(this);
    this.addMenuItem = this.addMenuItem.bind(this);
}
  
    componentDidMount(){
        this.getDataById();
      }
      
      getDataById(){
        const address = `api/Restaurants/${this.state.location}`;
        fetch(address)
        .then(res => res.json())
        .then((result) => {

          if(result.status === 200){
          
              let IsOwnerLogIn = false;
              if(result.restaurant.gotOwner === true){
                if(result.restaurant.ownerId === sessionStorage.getItem("id")){
                  IsOwnerLogIn= true;
                }
              }else{
                IsOwnerLogIn= true;
              }

              this.setState(prevState => ({
                restaurant: {
                    ...prevState.restaurant,
                id:result.restaurant.id,
                name: result.restaurant.name,
                address:result.restaurant.address,
                menu:result.restaurant.menu,
                gallery:result.restaurant.gallery,
                reviews:result.restaurant.reviews,
                rate:result.restaurant.score,
                ownerId:result.restaurant.ownerId,
                gotOwner:result.restaurant.gotOwner,
                lat:result.restaurant.lat,
                lng:result.restaurant.lng
                },
                ownerLogIn:IsOwnerLogIn
            }));
          }else{
            this.context.router.history.push(`/NieZnaleziono`);
          }


        })
        .catch((error) => {
          console.log(error);
        });
      }

 

       sendReview(event) {
         
        event.preventDefault();
         const adr =`api/Restaurants/${this.state.location}/AddReview`;
        
          fetch(adr, {
          method: 'PUT',
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization':'Bearer ' + sessionStorage.getItem("token")

          },
          body: JSON.stringify({
            description:this.state.newReview,
            restaurantId : this.state.restaurant.id,
            userId:sessionStorage.getItem("id")
          })
        }).then(res => res.json())
        .then((data) => {
          console.log(data.status);
          if(data.status === 200){
            console.log(JSON.stringify(data.newReview));
              let reviewsTmp =this.state.restaurant.reviews;
              reviewsTmp.unshift(data.newReview);
              this.setState(prevState => ({
              restaurant: {
                  ...prevState.restaurant,
                  reviews: reviewsTmp
              }
            }));
          }else{
            this.setState({
              responseMessageReviews:data.message
            });
          }
        
        } )
        .catch((err)=>console.log(err))  
      } 


      
      handleInputChange(event) {
        const target = event.target;
        const value = target.value;
        const name = target.name;
    
        this.setState({
          [name]: value
        });

      }


      backToRestaurationList() {
         this.context.router.history.push(`/ListaRestauracji/${this.state.restaurant.address.streetName}`); // Tu musi byc adress bez numerkow, albo z swerwera podzielone albo ja moge dzielic
       }
 
       static contextTypes = {
         router: PropTypes.object
       }



       /* ############## EDITION MODE ################## */

      handleRemoveMenuItem(e,addressToFetch){       
        const value = e.target.value;
        const adr =`api/Restaurants/${addressToFetch}`;

          fetch(adr, {
              method: 'DELETE',
              headers: {
                  'Accept': 'application/json',
                  'Content-Type': 'application/json',
                  'Authorization':'Bearer ' + sessionStorage.getItem("token")

              },
              body: JSON.stringify()
          }).then((data) => {

            if(data.status === 200){
              let index = this.state.restaurant.menu.findIndex((item)=>item.id == value); // Celowo ==
              if ( index !== -1){ 
                  this.state.restaurant.menu.splice(index,1);
                  this.setState({
                    menu: this.state.restaurant.menu
                  });
              }
            }else{
              this.setState({
                responseMessageRemoveMenuItem: data.message
              });
            }

          });

      }

      
/* ################################## POCZĄTEK STREFY DUZYCH ZMIAN ########################## */
     /* SendRestaurantInfo(event,name,value,addressToFetch) {
       
         event.preventDefault();
         const adr =`api/Restaurants/${addressToFetch}`;
        
         fetch(adr, {
                 method: "POST",
                 headers: {
                     'Accept': 'application/json',
                     'Content-Type': 'application/json',
                     'Authorization':'Bearer ' + sessionStorage.getItem("token")
                 },
                 body: JSON.stringify(
                     value
                 )
             }).then(res => res.json())
             .then((data) => {    
                 let flag=0;
                if(name === "menu"){
                  this.state.restaurant.menu.push(data);
                 }
                 else if (name === "gallery") {
                  this.state.restaurant.gallery.push(data);
                 }
                 else {
                     flag = 1;
                 }

                 this.setState(prevState => ({
                  restaurant: {
                      ...prevState.restaurant,
                      [name]:(flag === 1)?data:this.state.restaurant[name]
                  }
                }));


        
             } )
             .catch((err)=>console.log(err))
        
     } */



     addMenuItem(event,value,addressToFetch) {
       
      event.preventDefault();
      const adr =`api/Restaurants/${addressToFetch}`;
     
      fetch(adr, {
              method: "POST",
              headers: {
                  'Accept': 'application/json',
                  'Content-Type': 'application/json',
                  'Authorization':'Bearer ' + sessionStorage.getItem("token")
              },
              body: JSON.stringify(
                  value
              )
          }).then(res => res.json())
          .then((data) => {    
              
            if(data.status === 200){
              this.state.restaurant.menu.push(data.newMenuItem);
                this.setState(prevState => ({
               restaurant: {
                   ...prevState.restaurant,
                   address:this.state.restaurant.menu
               }
              }));
            }else{             
              this.setState({
                responseMessageMenuAddItem:data.message
            });

            }
     
          } )
          .catch((err)=>console.log(err))
     
  }






     

     changeAddress(event,value,addressToFetch) {
       
      event.preventDefault();
      const adr =`api/Restaurants/${addressToFetch}`;
     
      fetch(adr, {
              method: "POST",
              headers: {
                  'Accept': 'application/json',
                  'Content-Type': 'application/json',
                  'Authorization':'Bearer ' + sessionStorage.getItem("token")
              },
              body: JSON.stringify(
                  value
              )
          }).then(res => res.json())
          .then((data) => {    
              
            if(data.status === 200){
                this.setState(prevState => ({
               restaurant: {
                   ...prevState.restaurant,
                   address:data.newAddress
               }
              }));
            }else{             
              this.setState({
                responseMessageChangeAddress:data.message
            });

            }
     
          } )
          .catch((err)=>console.log(err))
     
  }








     changeName(event,value,addressToFetch) {
       
      event.preventDefault();
      const adr =`api/Restaurants/${addressToFetch}`;
     
      fetch(adr, {
              method: "POST",
              headers: {
                  'Accept': 'application/json',
                  'Content-Type': 'application/json',
                  'Authorization':'Bearer ' + sessionStorage.getItem("token")
              },
              body: JSON.stringify(
                  value
              )
          }).then(res => res.json())
          .then((data) => {    
             
            if(data.status === 200){
              this.setState(prevState => ({
               restaurant: {
                   ...prevState.restaurant,
                   name:data.name
               }
             }));
            }else{
              this.setState({
                responseMessageChangeName:data.message
            });
            }
     
          } )
          .catch((err)=>console.log(err))
     
  }
/* ################################## KONIEC STREFY DUZYCH ZMIAN ########################## */

     /* Upload Gallery */
     uploadJustFile(e) {
      e.preventDefault();
      let state = this.state;
      if (!state.hasOwnProperty('files')) {
          return;
      }

      let form = new FormData();

      for (var index = 0; index < state.files.length; index++) {
          var element = state.files[index];
          form.append('file', element);
      }

      axios.post(`api/Image/${this.state.restaurant.id}/Upload`, form)
          .then((result) => {


            if(result.status === 200){
              this.state.restaurant.gallery.push(result.data);
            this.setState(prevState => ({
              restaurant: {
                  ...prevState.restaurant,
                 gallery: this.state.restaurant.gallery
              }
            }));
          }else{
            this.setState({
              responseMessageUploadImage:result.message
          });   
          }


          })
          .catch((ex) => {
              console.error(ex);
          });
  }

 

  filesOnChange(sender) {
      let files = sender.target.files;
      let state = this.state;

      this.setState({
          ...state,
          files: files
      });
  }

  handleImageRemove(e) {
    e.preventDefault();
    const id = e.target.value;
    const adr =`api/Image/Delete/${id}`;

      fetch(adr, {
          method: 'DELETE',
          headers: {
              'Accept': 'application/json',
              'Content-Type': 'application/json',
              'Authorization':'Bearer ' + sessionStorage.getItem("token")

          },
          body: JSON.stringify()
      }).then((data) => {

       if(data.status === 200){   
         let index = this.state.restaurant.gallery.findIndex((img)=>img.id == id); 
          if ( index !== -1){  
            this.state.restaurant.gallery.splice(index,1);
            this.setState(prevState => ({
              restaurant: {
                  ...prevState.restaurant,
                  gallery: this.state.restaurant.gallery
              }
            }));
          }
        }else{
          this.setState({
            responseMessageRemoveImage:data.message
        });
        }
        
      

      });

  } 
    

       /* ########################################### */


       makeReport(event) {
         
         event.preventDefault();
         const adr =`/api/Notifications/${event.target.name}`;
         const  id = event.target.name === "CreateReportRestaurant"?this.state.restaurant.id:event.target.value;
        
          fetch(adr, {
          method: 'POST',
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization':'Bearer ' + sessionStorage.getItem("token")
          },
          body: JSON.stringify(
            id
            )
        }).then(res => res.json())
        .then((data) => { 
            alert(data.message);
        } )
        .catch((err)=>console.log(err))  
      } 


     removeRestaurant(e) {
        e.preventDefault();
        let res = this.state.responseMessage;
        const adr =`api/Restaurants/${this.state.restaurant.id}`;
    
          fetch(adr, {
              method: 'DELETE',
              headers: {
                  'Accept': 'application/json',
                  'Content-Type': 'application/json',
                  'Authorization':'Bearer ' + sessionStorage.getItem("token")
    
              },
              body: JSON.stringify()
          }).then((data) => {
            switch(data.status){
              case 200:               
                this.context.router.history.push(`/`);
              break;
              default:
                  res = data.message;
          }
          this.setState({
          responseMessage: res
            
        });


        });
    
      } 


    


  render() {
    return (
      <div className="singleRestaurant">
        {this.state.responseMessage}
        {(sessionStorage.getItem("role") === "Admin")?<button onClick={this.removeRestaurant} >Usuń restauracje</button>:null}
        <button  onClick={this.makeReport} name="CreateReportRestaurant">Zgłoś nieprawidłowości na stronie</button>
        <div className="header">
          <button className="back wow fadeInDown" data-wow-duration="2s" onClick={this.backToRestaurationList}>Powrót do listy</button>        
          <HeaderImage />
          <div className="title wow fadeInDown" data-wow-duration="1s">
            <RestaurantName responseMessageChangeName={this.state.responseMessageChangeName} ownerLogIn={this.state.ownerLogIn} name={this.state.restaurant.name} setName={this.changeName} restaurantId={this.state.restaurant.id}/>
          </div>
        </div>
        <div className="entryContent">
          <section className="localization">
              <MapSection lng={this.state.restaurant.lng} lat={this.state.restaurant.lat} ownerLogIn={this.state.ownerLogIn} address={this.state.restaurant.address} restaurantId={this.state.restaurant.id} updateAddress={this.changeAddress}  />
          </section>
          <section className="importantInformation">
              <Gallery responseMessageRemoveImage={this.responseMessageRemoveImage} responseMessageUploadImage={this.state.responseMessageUploadImage} ownerLogIn={this.state.ownerLogIn} addImage={this.handleImageAdd} removeImage={this.handleImageRemove} gallery={this.state.restaurant.gallery} filesOnChange={this.filesOnChange} uploadJustFile = {this.uploadJustFile} />
              <Menu responseMessageMenuAddItem={this.state.responseMessageMenuAddItem} responseMessageRemoveMenuItem={this.state.responseMessageRemoveMenuItem}  ownerLogIn={this.state.ownerLogIn} restaurantId={this.state.restaurant.id} addMenuItem={this.addMenuItem}  deleteMenuItem={this.handleRemoveMenuItem}   menu={this.state.restaurant.menu} />
          </section>
          <section className="reviews">
            {this.state.responseMessageReviews}
            {sessionStorage.getItem("id")?<ReviewsCreator onReviewInput={this.handleInputChange} onSendReview={this.sendReview}/>:null}
            <ReviewsList makeReport={this.makeReport}  reviews={this.state.restaurant.reviews}/>            
          </section>
        </div>
      </div>
    )
  }
}
