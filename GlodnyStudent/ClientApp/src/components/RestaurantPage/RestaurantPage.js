import React, { Component } from 'react';
import './RestaurantPage.css';
import ReviewsCreator from './ReviewsCreator';
import Rateing from './Rateing';
import PropTypes from 'prop-types';
import imageRestaurant from'../assets/restaurantImage.jpg';

export default class RestaurantPage extends Component {


constructor(props){
    super(props);
    this.state={
        location:window.location.href.slice(window.location.href.lastIndexOf("/")+1),
        name:"",
        address:"",
        menu:[],
        gallery:[],
        reviews:[],
        rate:0,
        newReview:'',
        /* Edycja */
        /* headerImage:null, */
        file: null,
        imagePreviewUrl: null
    };
    this.handleInputChange = this.handleInputChange.bind(this);
    this.sendReview =this.sendReview.bind(this);
    this.sendRate = this.sendRate.bind(this);
    this.backToRestaurationList = this.backToRestaurationList.bind(this);

    this.handleImageChange =this.handleImageChange.bind(this);
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
            gallery:result.gallery,
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

        event.preventDefault();
         const adr = (event.target.value === "Down")? `api/Restaurants/${this.state.location}/DownVote`:`api/Restaurants/${this.state.location}/UpVote`;
        
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



       /* ########################################## */

       handleImageChange(e) {
        e.preventDefault();
    
        let reader = new FileReader();
        let file = e.target.files[0];
    
        reader.onloadend = () => {
          this.setState({
            file: file,
            imagePreviewUrl: reader.result
          });
        }
    
        reader.readAsDataURL(file)
      }
       /* ########################################### */


  render() {
  const menuList = this.state.menu.map(row=><li className="wow fadeIn" data-wow-duration="2s" key={row.id}><span>{row.name}</span> <span className="price">{row.price}</span></li>);
  const reviewsList = this.state.reviews.map(row=>
    <div className="singleReview wow fadeIn" data-wow-duration="1s" key={row.id}>
      <div className="details">
        <ul>
          <li><i className="far fa-user"></i> Użyszkownik</li>
          <li>{row.addTime}</li>
        </ul>
      </div>
      <div className="description">
        {row.description}
      </div>
    </div>);
    const imageHeadre = this.state.imagePreviewUrl ? this.state.imagePreviewUrl :imageRestaurant;


    return (
      <div className="singleRestaurant">
        <div className="header">
          <button className="back wow fadeInDown" data-wow-duration="2s" onClick={this.backToRestaurationList}>Powrót do listy</button>
          <input type="file" name="headerImage"/>
          <div className="mainImage">
            <img src={imageHeadre} alt={this.state.name}/>
          </div>
          <div className="title wow fadeInDown" data-wow-duration="1s">
            <h2>{this.state.name}</h2>

            <Rateing rate={this.state.rate} onRate={this.sendRate}/>
          </div>
        </div>
        <div className="entryContent">
          <section className="localization">
              <div className="map">
              
              </div>
              <div className="mapInfo">
                  <h3 className="wow fadeIn" data-wow-duration="2s">Znajdź nas na mapie!</h3>
                  
                  <address className="wow fadeIn" data-wow-duration="2s">
                    <i className="fas fa-map-marker-alt fa-2x"></i> {this.state.address}
                  </address>

                  <button className="wow fadeIn" data-wow-duration="2s">Zobacz na mapach google</button>
              </div>
          </section>
          <section className="importantInformation">
              <div className="gallery">
                  <h3>Galeria</h3>

                  <ul>
                    <li className="wow fadeIn" data-wow-duration="2s"><a data-fancybox="gallery" href={imageRestaurant}><img src={imageRestaurant}/></a></li>
                    <li className="wow fadeIn" data-wow-duration="2s"><a data-fancybox="gallery" href={imageRestaurant}><img src={imageRestaurant}/></a></li>
                    <li className="wow fadeIn" data-wow-duration="2s"><a data-fancybox="gallery" href={imageRestaurant}><img src={imageRestaurant}/></a></li>
                    <li className="wow fadeIn" data-wow-duration="2s"><a data-fancybox="gallery" href={imageRestaurant}><img src={imageRestaurant}/></a></li>
                  </ul>
              </div>

              <div className="menu">
                <h3>Menu</h3>
                <ul>
                    {menuList}
                </ul>
              </div>
          </section>
          <section className="reviews">
              <div className="reviewsList">
                {reviewsList}
              </div> 
              <ReviewsCreator onReviewInput={this.handleInputChange} onSendReview={this.sendReview}/>
          </section>
        </div>
      </div>
    )
  }
}
