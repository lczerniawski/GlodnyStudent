import React, { Component } from 'react';
import uniqid from 'uniqid';

export default class Menu extends Component {


    constructor(props){
        super(props);
        this.state={
            id:null,
            name:"",
            price:null,
            disabledSubmit:true
        }
        this.inputValidate = this.inputValidate.bind(this);
    }


      inputValidate(event){
        
            let disable = true;
            let value = event.target.value;
            const name = event.target.name;

            if(name === "price"){
                disable = (!this.state.name && event.target.value);
            } else if(name === "name"){
                disable = !(this.state.price && value);
            }
             
            
            this.setState({
                disabledSubmit: disable,
                [name]:value
              });
      }


    render() {

        const menuList = this.props.menu.map(row=><li className="wow fadeIn" data-wow-duration="2s" key={row.id}>
            <span>{row.name}</span> <span className="price">{row.price}</span><button  value={row.id}  onClick={(e)=>this.props.removeMenuItem(e)}>-</button></li>);

        return (
            <div className="menu">
                <h3>Menu</h3>
                <ul>
                    {menuList}
                </ul>
                <label>Dodaj danie do menu
                    <form>
                       <label>Danie: <input type="text" name="name"  onChange={this.inputValidate} /></label>
                        <label>Cena: <input type="number" name="price" onChange={this.inputValidate} /> zł</label>
                        <input type="submit" disabled={this.state.disabledSubmit} onClick={(e)=>this.props.addMenuItem(e,uniqid(),this.state.name,this.state.price)} value="Dodaj"/>
                    </form>
                </label>               
              </div>
        )
    }
}
