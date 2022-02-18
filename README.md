# Code Extracts 

The purpose of this repo is to demonstrate my coding standards and ability. 

The repo provides three examples represented by the three folders:
- Data API
- Data Singleton 
- Json Rest API

## Data API

The scripts contained in this folder were used for a low-fidelity proof of concept. A Flask server was developed to serve as the REST API that provides data access to visual components of data visualizations for an AR Unity application. The REST API has basic authentication to protect against unauthorized access. The API is hosted on a virtual machine. 

The Data API contains dummy energy generation data for 2019. The generation data contains the energy generated per technology per hour. The data was received in CSV format, and then cleaned and restructured before being stored in a SQLite database for use in the API. A snippet of the data is provided below:

``` Note: energy data is given in MW and not all energy types are represented ```


| Date | Year | Month | Day | Hour | Coal | Nuclear | Wind | Total | 
| -- | --| -- | -- | -- | -- | -- | -- | -- |
| 2019-01-01 | 2019 | 1 | 1 | 0 | 20610 | 1789 | 0 | 22722 |
| 2019-01-01 | 2019 | 1 | 1 | 1 | 20169 | 1788 | 0 | 22118 |
| 2019-01-01 | 2019 | 1 | 1 | 2 | 19618 | 1785 | 0 | 21567 |


## Data Singleton

The Data Singleton controls the information that flows through to a data visualization rendered in Unity. The class is a Singleton, thus it will live from the start of the visual until the end, all other scripts (not available on the repo) can gain access from the sole instance of this class. 

The data used for this Singleton is the same data mentioned above in the Data API. The class stores the state of certain aspects like the month and maximum energy value per energy type per month, etc. 

Furthermore, the energyValDisplay script is provided to demonstrate the declaration of the singleton instance. The energyValDisplay streams data through and displays the energy values on a dashboard. 


## Json Rest API Scripting

This folder provides implementation of retrieving data from an external REST endpoint. The data is language translation. The proof of concept was aimed at overlaying translated phrases over target visual phrases using AR. The application used character recognition of just say an Afrikaans phrase, used this phrase as an input parameter request and the endpoint will respond with the translated phrase in the specified targeted language, let’s say English.  

