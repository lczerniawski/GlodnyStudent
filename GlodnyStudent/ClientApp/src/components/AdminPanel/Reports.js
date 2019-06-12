import React, { Component } from 'react';


export default class Reports extends Component {

    

    componentDidMount(e){
        this.props.getReports(e);
    }

    render() {
        const reportsList = this.props.reports.length !== 0?this.props.reports.map(report=><li id={report.id}>
            <button className="buttonDelete" value={report.id} onClick={(e)=>this.props.removeReport(e)} ><span>Usuń</span></button>
            <span className="detail">   
                <p><span className="ticketInfo">Informacja o zgłoszeniu:</span><br/>{report.content}</p>
                <p>Link: <a href={`/Restauracja/${report.restaurantId}`} >Przejdz do restauracji</a></p>
            </span>
            </li>):<li>Brak zgłoszeń</li>;
        return (
            <div className="tickets">
                <h3>Zgłoszenia</h3>
                <button className="refresh" onClick={this.props.getReports} >Odświerz listę zgłoszeń</button>
                <ul className="informations">
                    {reportsList}
                </ul>
            </div>
        )
    }
}
