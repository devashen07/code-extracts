import pandas as pd
import sqlite3

# Practice script for loading SQL table into a pandas df

# Read sqlite query results into a pandas DataFrame
conn = sqlite3.connect("../data.sqlite")
df = pd.read_sql_query("SELECT * from data_mix", conn)

conn.close()

df.index = df.Datetime

print(df.info())

# Verify that result of SQL query is stored in the dataframe
print(df.head())

