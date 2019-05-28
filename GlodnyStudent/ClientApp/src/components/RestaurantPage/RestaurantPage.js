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
import MapSection from './MapSection';
import RestaurantName from './RestaurantName';

export default class RestaurantPage extends Component {


constructor(props){
    super(props);
    this.state={
        location:window.location.href.slice(window.location.href.lastIndexOf("/")+1),
        name:"",
        address:"", // Do przerobienia na talice obiektów
        menu:[],
        gallery:[{id:uniqid(),file:"",dataUrl:imageRestaurant},{id:uniqid(),file:"",dataUrl:imageRestaurant},{id:uniqid(),file:"",dataUrl:imageRestaurant},{id:uniqid(),file:"",dataUrl:imageRestaurant}],
        reviews:[],
        rate:0,
        newReview:'',
        headerImage: null /* Z servera zwracane oddzielnie */

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
          this.setState({
            error:false,
            name: result.name,
            address:result.address,
            menu:result.menu,
    /*         gallery:result.gallery, */
            reviews:result.reviews,
            rate:result.score
          });
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
              
            this.setState({
            rate:  data.rating
            }); 

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
            reviewerId: "2137"
          })
        }).then(res => res.json())
        .then((data) => {
 
          this.state.reviews.push(data);

          this.setState({
            reviews:  this.state.reviews
           });
        
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
         this.context.router.history.push(`/ListaRestauracji/${this.state.address}`); // Tu musi byc adress bez numerkow, albo z swerwera podzielone albo ja moge dzielic
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
            this.state.gallery.push({id:uniqid(),file:file,dataUrl:reader.result});
            addToGallery = this.state.gallery;
          }
      
          this.setState({
            [name]: addToGallery? addToGallery:{id:uniqid(),file:file,dataUrl:reader.result}
          });
        }
        reader.readAsDataURL(file);
      }


       handleImageRemove(e) {
        e.preventDefault();
        const name = e.target.name;
        const value = e.target.value;
        let newValue = null;

           if(name === "gallery"){    
              let index = this.state.gallery.findIndex((img)=>img.id === value); 
              if ( index !== -1){  
                this.state.gallery.splice(index,1);
              }
            newValue = this.state.gallery;
           }

          this.setState({
            [name]: newValue
          });
      } 

      
      handleRemoveMenuItem(e){       
        const value = e.target.value;
        let index = this.state.menu.findIndex((item)=>item.id == value); // Celowo ==
        if ( index !== -1){ 
            this.state.menu.splice(index,1);
            this.setState({
              menu: this.state.menu
            });
        }       
      }

      handleAddMenuItem(e,id,name,price){
        e.preventDefault();
        this.state.menu.push({id,name,price});
        this.setState({
          menu: this.state.menu
        });     
      }

      updateRestaurantInfo(name,value){
        this.setState({
          [name]: value
        });
      }

       /* ########################################### */


  render() {


    return (
      <div className="singleRestaurant">
        <div className="header">
          <button className="back wow fadeInDown" data-wow-duration="2s" onClick={this.backToRestaurationList}>Powrót do listy</button>        
          <HeaderImage headerImage={this.state.headerImage} addImage={this.handleImageAdd} removeImage={this.handleImageRemove}/>
          <div className="title wow fadeInDown" data-wow-duration="1s">
            <RestaurantName name={this.state.name} setName={this.updateRestaurantInfo}/>
            <Rateing rate={this.state.rate} onRate={this.sendRate}/>
          </div>
        </div>
        <div className="entryContent">
          <section className="localization">
              <MapSection address={this.state.address} updateAddress={this.updateRestaurantInfo} />
          </section>
          <section className="importantInformation">
              <Gallery addImage={this.handleImageAdd} removeImage={this.handleImageRemove} gallery={this.state.gallery} />
              <Menu addMenuItem={this.handleAddMenuItem}  menu={this.state.menu} removeMenuItem={this.handleRemoveMenuItem} />
          </section>
          <section className="reviews">
            <ReviewsCreator onReviewInput={this.handleInputChange} onSendReview={this.sendReview}/>
            <ReviewsList reviews={this.state.reviews}/>            
          </section>
        </div>
      </div>
    )
  }
}
