import pandas as pd

from flask import Flask
from flask import request, jsonify
from flask_httpauth import HTTPBasicAuth
import sqlite3

#can add more passwords or update here if needed (the auth can be done more strictly but not that necessary now with public data

LOGIN_INFO = {
    "devashen": "password123"
}

# create the flask app
app = Flask(__name__)

# add basic auth
auth = HTTPBasicAuth()


# open a connection to the database
conn = sqlite3.connect("/home/data_service/data.sqlite")

# read the entire database table into memory (once off cost
# all operations can then occur directly on the df and not affect the stored data in any way
df = pd.read_sql_query("SELECT * from data_mix", conn,parse_dates=['Datetime'])

df.index = df.Datetime

print(df.info())

conn.close()


def dict_factory(cursor, row):
    d = {}
    for idx, col in enumerate(cursor.description):
        d[col[0]] = row[idx]
    return d


@auth.verify_password
def verify(username, password):
    """
    look up acceptable passwords in LOGIN INFO and allow access to authorised users only
    :param username:
    :param password:
    :return:
    """
    if not (username and password):
        return False
    return LOGIN_INFO.get(username) == password


@app.route('/', methods=['GET'])
@auth.login_required
def home():
    """
    root of the API, doesn't do much.
    Useful for verifying that the API is running

    This is an example call for this function:
    http://127.0.0.1:9000/

    :return:
    """
    return '''<h1>Data API</h1>
<p>Data to be consumed by Proof of Concept</p>'''



@app.route('/all_data', methods=['GET'])
@auth.login_required
def get_all_data_mix():
    """
    This function extracts all the data from the database
    and returns a jsonified object to the calling application

    This is an example call for this function:
    http://127.0.0.1:9000/all_data

    :return: all the generation data from the db
    """

    return(df.to_json(orient='records'))



@app.route('/data/daterange', methods=['GET'])
@auth.login_required
def get_date_range_data_mix():
    """
    This function will return data that fits within a date range.

    This is an example call for this function:
    http://127.0.0.1:9000/data/daterange?start=2010-01-01+00:00:00&end=2010-01-31+23:00:00

    Did not cater to multiple date formats here so please stick to the format
    yyyy-mm-dd hh:MM:ss (the + is added since no spaces can exist in a URL)

    :return: data that is within the range specified in the call
    """

    query_parameters = request.args

    start = query_parameters.get('start')
    end = query_parameters.get('end')

    # do some validation to check if the start and end date is specified

    if start is None:
        return page_not_found(404)
    if end is None:
        return page_not_found(404)

    data = df.loc[start:end]

    # return the jsonified results
    return data.to_json(orient='records')


@app.route('/data/daterange/tech', methods=['GET'])
def get_date_range_data_mix_for_tech():
    """
    This function will return data that fits within a date range for specified technologies.

    This is an example call for this function:
    http://127.0.0.1:9000/data/daterange/tech?start=2010-01-01+00:00:00&end=2010-01-31+23:00:00&technologies=coal,nuclear

    (Devashen in text the date formats need to be in the following format: 2010-01-01 00:00:00)

    Did not cater to multiple date formats here so please stick to the format
    yyyy-mm-dd hh:MM:ss (the + is added since no spaces can exist in a URL)


    These are the different technology names as stored in the DB

    "CSP": 0,
    "Coal": 20337,
    "Datetime": "2015-01-01 00:00:00",
    "Gas_Turbine(inclIPP)": -2,
    "InternationalImports": 1310,
    "Largescalehydro": 0,
    "Loadshedding": 0,
    "Nuclear": 1858,
    "Otherrenewable": 0,
    "PV": 0,
    "PumpedStorage": -1329,
    "ShortTermIPPs": 329,
    "TotalEskomsent-out(afterPSL)": 22619,
    "Wind": 116

    Reference them as they are or update the db (case is not important for SQL)

    :return: data that is within the range specified in the call
    """

    query_parameters = request.args

    start = query_parameters.get('start')
    end = query_parameters.get('end')
    tech = query_parameters.get('technologies')

    tech_list = tech.split(",")

    # do some validation to check if the start and end date is specified

    if start is None:
        return page_not_found(404)
    if end is None:
        return page_not_found(404)
    if tech is None:
        return page_not_found(404)


    data = df.loc[start:end]

    # capitalise first letter (this is done since browser makes it all lowercase and df is case sensitive
    # might need to remove this if it causes problems
    tech_list = [item.capitalize() for item in tech_list]


    # we want the time to be available so add it to the list of things to return
    tech_list.insert(0, "Datetime")

    # return the jsonified results
    return data[tech_list].to_json(orient='records')


@app.route('/data/daterange/tech/average/year', methods=['GET'])
def get_average_per_year_for_tech():
    """
    This function will return data that fits within a date range for specified technologies.

    This is an example call for this function:
    http://127.0.0.1:9000/data/daterange/tech/average/year?start=2010&end=2015&technologies=coal,nuclear


    Here you can just specify the years for filtering, don't need the whole date


    These are the different technology names as stored in the DB

    "CSP": 0,
    "Coal": 20337,
    "Datetime": "2015-01-01 00:00:00",
    "Gas_Turbine(inclIPP)": -2,
    "InternationalImports": 1310,
    "Largescalehydro": 0,
    "Loadshedding": 0,
    "Nuclear": 1858,
    "Otherrenewable": 0,
    "PV": 0,
    "PumpedStorage": -1329,
    "ShortTermIPPs": 329,
    "TotalEskomsent-out(afterPSL)": 22619,
    "Wind": 116

    Reference them as they are or update the db (case is not important for SQL)

    :return: data that is within the range specified in the call
    """

    query_parameters = request.args

    start = query_parameters.get('start')
    end = query_parameters.get('end')
    tech = query_parameters.get('technologies')

    print(tech)

    tech_list = tech.split(",")

    # do some validation to check if the start and end date is specified

    if start is None:
        return page_not_found(404)
    if end is None:
        return page_not_found(404)
    if tech is None:
        return page_not_found(404)

    data = df.loc[start:end]

    # capitalise first letter (this is done since browser makes it all lowercase and df is case sensitive
    # might need to remove this if it causes problems
    tech_list = [item.capitalize() for item in tech_list]

    data2 = data.groupby(data.Datetime.dt.year)[tech_list].mean()
    data2['Date'] = data2.index

    print(data2.head())

    # return the jsonified results
    return data2.to_json(orient='records')


@app.route('/data/daterange/tech/average/month', methods=['GET'])
def get_average_per_month_for_tech():
    """
    This function will return data that fits within a date range for specified technologies.

    This is an example call for this function:
    http://127.0.0.1:9000/data/daterange/tech/average/month?start=2010&end=2015&technologies=coal,nuclear


    Here you can just specify the years and months (2020-01) for filtering, don't need the whole date


    These are the different technology names as stored in the DB

    "CSP": 0,
    "Coal": 20337,
    "Datetime": "2015-01-01 00:00:00",
    "Gas_Turbine(inclIPP)": -2,
    "InternationalImports": 1310,
    "Largescalehydro": 0,
    "Loadshedding": 0,
    "Nuclear": 1858,
    "Otherrenewable": 0,
    "PV": 0,
    "PumpedStorage": -1329,
    "ShortTermIPPs": 329,
    "TotalEskomsent-out(afterPSL)": 22619,
    "Wind": 116

    Reference them as they are or update the db (case is not important for SQL)

    :return: data that is within the range specified in the call
    """

    query_parameters = request.args

    start = query_parameters.get('start')
    end = query_parameters.get('end')
    tech = query_parameters.get('technologies')

    print(tech)

    tech_list = tech.split(",")

    # do some validation to check if the start and end date is specified

    if start is None:
        return page_not_found(404)
    if end is None:
        return page_not_found(404)
    if tech is None:
        return page_not_found(404)

    data = df.loc[start:end]

    # capitalise first letter (this is done since browser makes it all lowercase and df is case sensitive
    # might need to remove this if it causes problems
    tech_list = [item.capitalize() for item in tech_list]

    data2 = data.groupby([data.Datetime.dt.year,data.Datetime.dt.month])[tech_list].mean()

    datestrings = []
    for x in data2.index:
        if x[1] >= 10:
            datestrings.append(str(x[0]) + "-" + str(x[1]))
        else:
            datestrings.append(str(x[0]) + "-0" + str(x[1]))


    data2['Date'] = datestrings

    print(data2.head())

    # return the jsonified results
    return data2.to_json(orient='records')




# Handle different error handling routes below

@app.errorhandler(404)
def page_not_found(e):
    print(e)
    return "<h1>"+e.__str__()+"</h1><h1>Sure you know what you're doing?</h1>", 404



# the machine "virtual_machine_url.co.za" has the following open ports 9000-9004
# use one of these otherwise it won't work externally
app.run(host='virtual_machine_url.co.za', port=9000)

