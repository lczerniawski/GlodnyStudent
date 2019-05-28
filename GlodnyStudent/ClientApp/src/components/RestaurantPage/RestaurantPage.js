import React, { Component } from 'react';
import './RestaurantPage.css';
import ReviewsCreator from './ReviewsCreator';
import Rateing from './Rateing';
import PropTypes from 'prop-types';
import uniqid from 'uniqid';
import Gallery from './Gallery';
import Menu from './Menu';
import ReviewsList from './ReviewsList';
import HeaderImage from './HeaderImage';
import imageRestaurant from'../assets/restaurantImage.jpg';
import Map from './Map';
import RestaurantName from './RestaurantName';

export default class RestaurantPage extends Component {


constructor(props){
    super(props);
    this.state={
        location:window.location.href.slice(window.location.href.lastIndexOf("/")+1),
      /*   name:"",
        address:[], // Do przerobienia na talice obiektów
        menu:[],
        gallery:[{id:uniqid(),file:"",dataUrl:imageRestaurant},{id:uniqid(),file:"",dataUrl:imageRestaurant},{id:uniqid(),file:"",dataUrl:imageRestaurant},{id:uniqid(),file:"",dataUrl:imageRestaurant}],
        reviews:[],
        rate:0, */
        newReview:'',
        headerImage: null, /* Z servera zwracane oddzielnie */   // NIEPOZMIENIANE
        restaurant:{
          id:0,
          address:{street:"",streetNumber:"",localNumber:0,district:""},
          gallery:[],
          menu:[],
          name:"",
          reviews:[],
          rate:0,
        }

    };
    this.handleInputChange = this.handleInputChange.bind(this);
    this.sendReview =this.sendReview.bind(this);
    this.sendRate = this.sendRate.bind(this);
    this.backToRestaurationList = this.backToRestaurationList.bind(this);

    this.handleImageAdd =this.handleImageAdd.bind(this);
    this.handleImageRemove = this.handleImageRemove.bind(this);
    this.handleRemoveMenuItem = this.handleRemoveMenuItem.bind(this);
    this.handleAddMenuItem = this.handleAddMenuItem.bind(this);
    this.updateRestaurantInfo = this.updateRestaurantInfo.bind(this);
}
  
  

    componentDidMount(){
      this.getDataById();
      /* console.log(`restaurant1: ${JSON.stringify(this.state.restaurant)}`); */
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
          /* this.state.address.push(result.address);
          this.setState({
            error:false,
            name: result.name,
            address:this.state.address,
            menu:result.menu,
            gallery:result.gallery,
            reviews:result.reviews,
            rate:result.score
          }); */

          /* console.log(`result: ${JSON.stringify(result)}`); */
         /*  this.setState({
            error:false,
            restaurant:result.restaurant
          });  */


          this.setState(prevState => ({
            restaurant: {
                ...prevState.restaurant,
            id:result.id,
            name: result.name,
            address:result.address,
            menu:result.menu,
            gallery:result.gallery,
            reviews:result.reviews,
            rate:result.score
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
            }
          }).then(res => res.json())
          .then((data) => {
              
           /*  this.setState({
            rate:  data.rating
            });  */

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
          },
          body: JSON.stringify({
            description:this.state.newReview,
           /*  reviewerId: "2137", */
            restaurantId : this.state.restaurant.id
          })
        }).then(res => res.json())
        .then((data) => {
 
          this.state.restaurant.reviews.push(data);

         /*  this.setState({
            reviews:  this.state.reviews
           }); */

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

       handleImageAdd(e) {
        e.preventDefault();
        const name = e.target.name;
        let reader = new FileReader();
        let file = e.target.files[0];
        let addToGallery = null;
 
        reader.onloadend = () => {
          if(name === "gallery"){
            this.state.restaurant.gallery.push({id:uniqid(),file:file,dataUrl:reader.result});
            addToGallery = this.state.restaurant.gallery;
          }
      
         /*  this.setState({
            [name]: addToGallery? addToGallery:{id:uniqid(),file:file,dataUrl:reader.result}
          }); */
          

          this.setState(prevState => ({
            restaurant: {
                ...prevState.restaurant,
                [name]: addToGallery? addToGallery:{id:uniqid(),file:file,dataUrl:reader.result}
            }
          }));



        }
        reader.readAsDataURL(file);
      }


       handleImageRemove(e) {
        e.preventDefault();
        const name = e.target.name;
        const value = e.target.value;
        let newValue = null;

           if(name === "gallery"){    
              let index = this.state.restaurant.gallery.findIndex((img)=>img.id === value); 
              if ( index !== -1){  
                this.state.restaurant.gallery.splice(index,1);
              }
            newValue = this.state.restaurant.gallery;
           }

          /* this.setState({
            [name]: newValue
          }); */

          this.setState(prevState => ({
            restaurant: {
                ...prevState.restaurant,
                [name]: newValue
            }
          }));



      } 

      
      handleRemoveMenuItem(e){       
        const value = e.target.value;
        let index = this.state.restaurant.menu.findIndex((item)=>item.id == value); // Celowo ==
        if ( index !== -1){ 
            this.state.restaurant.menu.splice(index,1);
            this.setState({
              menu: this.state.restaurant.menu
            });
        }       
      }

      handleAddMenuItem(e,id,name,price){
        e.preventDefault();
        this.state.menu.restaurant.push({id,name,price});
       /*  this.setState({
          menu: this.state.restaurant.menu
        });    */  

        this.setState(prevState => ({
          restaurant: {
              ...prevState.restaurant,
              menu: this.state.restaurant.menu
          }
        }));

        
      }

      updateRestaurantInfo(adr){
        /* this.setState({
          address: adr
        }); */


        this.setState(prevState => ({
          restaurant: {
              ...prevState.restaurant,
              address: adr
          }
        }));



      }

     /*  updateRestaurantInfo(event,method,names,values) {
       
        event.preventDefault();
         const adr =`api/Restaurants/${this.state.location}`;
        
          fetch(adr, {
          method: {method},
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
              name:this.state.name,
              address:this.state.address,
              menu:this.setState.menu,
              gallery:this.state.gallery,
              reviews:this.state.reviews,
              rate:this.state.reviews,
              headerImage:this.state.headerImage
          })
        }).then(res => res.json())
        .then((data) => {
 
          this.state.reviews.push(data.reviews);
          this.state.reviews.push(data.menu);
          this.state.reviews.push(data.r);

          this.setState({
              name:this.state.name,
              address:this.state.address,
              menu:this.setState.menu,
              gallery:this.state.gallery,
              reviews:this.state.reviews,
              rate:this.state.reviews,
              headerImage:this.state.headerImage
           });
        
        } )
        .catch((err)=>console.log(err))
        
      }  */ 

       /* ########################################### */


  render() {
        /* console.log(`restaurant2: ${JSON.stringify(this.state.restaurant)}`); */
    return (
      <div className="singleRestaurant">
        <div className="header">
          <button className="back wow fadeInDown" data-wow-duration="2s" onClick={this.backToRestaurationList}>Powrót do listy</button>        
          <HeaderImage headerImage={this.state.headerImage} addImage={this.handleImageAdd} removeImage={this.handleImageRemove}/>
          <div className="title wow fadeInDown" data-wow-duration="1s">
            <RestaurantName name={this.state.restaurant.name} setName={this.updateRestaurantInfo}/>
            <Rateing rate={this.state.restaurant.rate} onRate={this.sendRate}/>
          </div>
        </div>
        <div className="entryContent">
          <section className="localization">

              <Map address={this.state.restaurant.address} updateAddress={this.updateRestaurantInfo} />
          </section>
          <section className="importantInformation">
              <Gallery addImage={this.handleImageAdd} removeImage={this.handleImageRemove} gallery={this.state.restaurant.gallery} />
              <Menu addMenuItem={this.handleAddMenuItem}  menu={this.state.restaurant.menu} removeMenuItem={this.handleRemoveMenuItem} />
          </section>
          <section className="reviews">
            <ReviewsCreator onReviewInput={this.handleInputChange} onSendReview={this.sendReview}/>
            <ReviewsList reviews={this.state.restaurant.reviews}/>            
          </section>
        </div>
      </div>
    )
  }
}
