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
        name:"",
        address:"",
        menu:[],
        gallery:[],
        reviews:[],
        rate:0,
        newReview:''
    };
    this.handleInputChange = this.handleInputChange.bind(this);
    this.sendReview =this.sendReview.bind(this);
    this.sendRate = this.sendRate.bind(this);
    this.backToRestaurationList = this.backToRestaurationList.bind(this);
}
  
    componentDidMount(){
        this.getDataById();
      }
      
      getDataById(){
        const address = `api/Restaurants/${this.props.id}`;
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
         const adr = (event.target.value === "Down")? `api/Restaurants/${this.props.id}/DownVote`:`api/Restaurants/${this.props.id}/UpVote`;
        
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
         const adr =`api/Restaurants/${this.props.id}/AddReview`;
        
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
         this.context.router.history.push(`/ListaRestauracji`);
       }
 
       static contextTypes = {
         router: PropTypes.object
       }



  render() {
  const menuList = this.state.menu.map(row=><li className="wow fadeIn" data-wow-duration="1s" key={row.id}><span>{row.name}</span> <span className="price">{row.price}</span></li>);
  const reviewsList = this.state.reviews.map(row=>
    <div className="singleReview wow fadeIn" data-wow-duration="1s" key={row.id}>
      <div className="details">
        <ul>
          <li><i class="far fa-user"></i> Użyszkownik</li>
          <li>{row.addTime}</li>
        </ul>
      </div>
      <div className="description">
        {row.description}
      </div>
    </div>);

    return (
      <div className="singleRestaurant">
        <div className="header">
          <button className="back wow fadeInDown" data-wow-duration="1s" onClick={this.backToRestaurationList}>Powrót do listy</button>

          <div className="mainImage">
            <img src={imageRestaurant} alt={this.state.name}/>
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
                  <h3 className="wow fadeIn" data-wow-duration="1s">Znajdź nas na mapie!</h3>
                  
                  <address className="wow fadeIn" data-wow-duration="1s">
                    <i className="fas fa-map-marker-alt fa-2x"></i> {this.state.address}
                  </address>

                  <button className="wow fadeIn" data-wow-duration="1s">Zobacz na mapach google</button>
              </div>
          </section>
          <section className="importantInformation">
              <div className="gallery">
                  <h3>Galeria</h3>

                  <ul>
                    <li className="wow fadeIn" data-wow-duration="1s"><a data-fancybox="gallery" href={imageRestaurant}><img src={imageRestaurant}/></a></li>
                    <li className="wow fadeIn" data-wow-duration="1s"><a data-fancybox="gallery" href={imageRestaurant}><img src={imageRestaurant}/></a></li>
                    <li className="wow fadeIn" data-wow-duration="1s"><a data-fancybox="gallery" href={imageRestaurant}><img src={imageRestaurant}/></a></li>
                    <li className="wow fadeIn" data-wow-duration="1s"><a data-fancybox="gallery" href={imageRestaurant}><img src={imageRestaurant}/></a></li>
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
              <ReviewsCreator onReviewInput={this.handleInputChange} onSendReview={this.sendReview}/>
              <div className="reviewsList">
                {reviewsList}
              </div> 
          </section>
        </div>
      </div>
    )
  }
}
