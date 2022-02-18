import sqlite3
import pandas as pd

# read in the pandas df
df = pd.read_csv("../data.csv",parse_dates=["Datetime"])

print(df.head())
print(df.info())

# create a connection to sql db
conn = sqlite3.connect('../data.sqlite')

# write the dataframe to the sqlite db
df.to_sql('data_mix', conn, if_exists='replace', index=False)

# view the contents of the db to validate that it worked
print(pd.read_sql('select * from data_mix', conn))