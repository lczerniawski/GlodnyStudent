import React, { Component } from 'react';
import './RestaurantPage.css';
import ReviewsCreator from './ReviewsCreator';
import Rateing from './Rateing';
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
          fields: {},
          ownerLogIn:false,
          location:window.location.href.slice(window.location.href.lastIndexOf("/")+1),
          newReview:'',
          restaurant:{
            id:0,
            address:{street:"",streetNumber:"",localNumber:0,district:""},
            gallery:[],
            menu:[],
            name:"",
            reviews:[],
            rate:0,
            ownerId:null,
            gotOwner:false
          }

    };
    this.handleInputChange = this.handleInputChange.bind(this);
    this.sendReview =this.sendReview.bind(this);
    this.sendRate = this.sendRate.bind(this);
    this.backToRestaurationList = this.backToRestaurationList.bind(this);
    this.handleImageRemove = this.handleImageRemove.bind(this);
    this.handleRemoveMenuItem = this.handleRemoveMenuItem.bind(this);
    this.SendRestaurantInfo = this.SendRestaurantInfo.bind(this);
    this.uploadJustFile = this.uploadJustFile.bind(this);
    this.filesOnChange = this.filesOnChange.bind(this);
}
  
    componentDidMount(){
        this.getDataById();
        if(this.state.restaurant.gotOwner === true){
          if(this.state.restaurant.ownerId === sessionStorage.getItem("id")){
            this.setState({ownerLogIn:true});
          }
        }
      }
      
      getDataById(){
        const address = `api/Restaurants/${this.state.location}`;
        fetch(address).then((response) => {
          if (response.ok) {
            return response.json();
          } else {
            throw new Error('Restauration not found');
          }
        })
        .then((result) => {
          this.setState(prevState => ({
            restaurant: {
                ...prevState.restaurant,
            id:result.id,
            name: result.name,
            address:result.address,
            menu:result.menu,
            gallery:result.gallery,
            reviews:result.reviews,
            rate:result.score,
            ownerId:result.ownerId,
            gotOwner:result.gotOwner
            }
        }));


        })
        .catch((error) => {
          console.log(error);
          this.setState({
            error
          }); 
        });
      }

 
      sendRate(event) {

       
         let adr = null;
         if(event.target.value === "Down"){
            adr =`api/Restaurants/${this.state.location}/DownVote`
         } else if (event.target.value === "Up"){
          adr = `api/Restaurants/${this.state.location}/UpVote`;
         }
        
         if(adr != null){
            fetch(adr, {
            method: 'POST',
            headers: {
              'Accept': 'application/json',
              'Content-Type': 'application/json',
              'Authorization':'Bearer ' + sessionStorage.getItem("token")

            }
          }).then(res => res.json())
          .then((data) => {            
            this.setState(prevState => ({
              restaurant: {
                  ...prevState.restaurant,
                  rate:  data.rating
              }
          }));

          } )
          .catch((err)=>console.log(err))  
        }else {console.log(`Name: ${event.target.name}  value: ${event.target.value} `);}
        event.preventDefault();
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
            restaurantId : this.state.restaurant.id
          })
        }).then(res => res.json())
        .then((data) => {
 
          this.state.restaurant.reviews.unshift(data);
           this.setState(prevState => ({
            restaurant: {
                ...prevState.restaurant,
                reviews: this.state.restaurant.reviews
            }
          }));
        
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
         this.context.router.history.push(`/ListaRestauracji/${this.state.restaurant.address.street}`); // Tu musi byc adress bez numerkow, albo z swerwera podzielone albo ja moge dzielic
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

            let index = this.state.restaurant.menu.findIndex((item)=>item.id == value); // Celowo ==
            if ( index !== -1){ 
                this.state.restaurant.menu.splice(index,1);
                this.setState({
                  menu: this.state.restaurant.menu
                });
            }

          });

      }

      

     SendRestaurantInfo(event,name,value,addressToFetch) {
       
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
        
     }


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


            this.state.restaurant.gallery.push(result.data);
            this.setState(prevState => ({
              restaurant: {
                  ...prevState.restaurant,
                 gallery: this.state.restaurant.gallery
              }
            }));


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
        
      

      });

  } 
    

       /* ########################################### */


  render() {
        /* console.log(`restaurant2: ${JSON.stringify(this.state.restaurant)}`); */
    return (
      <div className="singleRestaurant">
        <div className="header">
          <button className="back wow fadeInDown" data-wow-duration="2s" onClick={this.backToRestaurationList}>Powrót do listy</button>        
          <HeaderImage />
          <div className="title wow fadeInDown" data-wow-duration="1s">
            <RestaurantName ownerLogIn={this.state.ownerLogIn} name={this.state.restaurant.name} setName={this.SendRestaurantInfo} restaurantId={this.state.restaurant.id}/>
            <Rateing rate={this.state.restaurant.rate} onRate={this.sendRate}/>
          </div>
        </div>
        <div className="entryContent">
          <section className="localization">
              <MapSection ownerLogIn={this.state.ownerLogIn} address={this.state.restaurant.address} restaurantId={this.state.restaurant.id} updateAddress={this.SendRestaurantInfo}  />
          </section>
          <section className="importantInformation">
              <Gallery ownerLogIn={this.state.ownerLogIn} addImage={this.handleImageAdd} removeImage={this.handleImageRemove} gallery={this.state.restaurant.gallery} filesOnChange={this.filesOnChange} uploadJustFile = {this.uploadJustFile} />
              <Menu  ownerLogIn={this.state.ownerLogIn} restaurantId={this.state.restaurant.id} addMenuItem={this.SendRestaurantInfo}  deleteMenuItem={this.handleRemoveMenuItem}   menu={this.state.restaurant.menu} />
          </section>
          <section className="reviews">
            {this.state.ownerLogIn?<ReviewsCreator onReviewInput={this.handleInputChange} onSendReview={this.sendReview}/>:null}
            <ReviewsList reviews={this.state.restaurant.reviews}/>            
          </section>
        </div>
      </div>
    )
  }
}
