import React, { Component } from 'react';
import './RestaurantPage.css';
import ReviewsCreator from './ReviewsCreator';
import Rateing from './Rateing';
import PropTypes from 'prop-types';

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
  const menuList = this.state.menu.map(row=><li key={row.id} className="menuRow"><span>{row.name}</span><span>{row.price}</span></li>);
  const reviewsList = this.state.reviews.map(row=><li key={row.id} className="menuRow"><span>{row.description}</span><span>{row.addTime}</span></li>);
    return (
      <div>
        <button onClick={this.backToRestaurationList}>Powrót do listy</button>
        <div >TU BEDZIE NAGŁÓWKOWY OBRAZEK</div>
        <div>{this.state.name}</div>
        <section>
            <div>MAPA</div>
            <div>
                <h3>Znajdź nas na mapie!</h3>
                <p>{this.state.address}</p>
                <Rateing rate={this.state.rate} onRate={this.sendRate}/>
                <button>Zobacz na mapach google</button>
            </div>
        </section>
            Tu będzie galleria
        <section>
        <section>
            Tu będzie menu
        </section>    
            <ul>
                {menuList}
            </ul>
        </section>
        <section>
            <ReviewsCreator onReviewInput={this.handleInputChange} onSendReview={this.sendReview}/>
            <ul>
            {reviewsList}
            </ul> 
        </section>
      </div>
    )
  }
}