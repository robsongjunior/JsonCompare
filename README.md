📝 JSON Compare Utility
Welcome to the JSON Compare Utility repository! 🚀

This project offers a tool to compare two JSON strings and identify the differences between them. It is ideal for tracking changes in structured data, such as logs or configurations, in a clear and organized manner.

🌟 Features
JSON Comparison: Compare two JSON strings and get all the differences.
Ignore Keys: Specify keys that should be ignored during the comparison.
Difference Details: Identify insertions, updates, and deletions of values.

📚 Documentation
CompareJson: Compares two JSON strings and returns the differences.
HasDifference: Checks if there are differences between two simple JSON strings.
DifferenceDetails: Details the differences found, categorizing them into insertions, updates, and deletions.

🔧 Requirements
Newtonsoft.Json: Ensure you have the Newtonsoft.Json package installed to use the tool.

🛠️ Usage Example
Imagine you are developing a user profile monitoring system and need to check for changes in profile data after updates.

Run the TestRun project and understand its functionality.

Before:

`{
    "Name": "John Doe",
    "Age": 30,
    "Address": {
        "Street": "123 Main St",
        "City": "Anytown",
        "State": "CA"
    },
    "Skills": ["C#", "SQL", "JavaScript"]
}`

After:
`{
    "Name": "John Doe",
    "Age": 31,
    "Address": {
        "Street": "123 Main St",
        "City": "New Haven",
        "State": "NY"
    },
    "Skills": ["C#", "SQL", "Python"]
}`

Expected Response:
`{
    "Age": {
        "updated": [31]
    },
    "Address": {
        "City": {
            "updated": ["New Haven"]
        }
    },
    "Skills": {
        "removed": ["JavaScript"],
        "added": ["Python"]
    }
}`

The compared values can also come from a database, using the more traditional JSON style with \ delimiting the JSON and not just dictionaries.

Benefits:
Traceability: Quickly identify changes in user profile data.
Efficiency: Reduce time spent analyzing large amounts of JSON data.
Flexibility: Ignore specific keys that are not relevant for the comparison.

🚀 Getting Started
Clone the repository: git clone https://github.com/your-user/json-compare-utility.git
Open the project in your preferred IDE.
Install the necessary dependencies.
Use the Compare class to compare your JSONs!

⚠️ I am providing this code as fully open, free, and without any liabilities for its use. The primary goal is to strengthen the open-source community and promote learning. This code is not intended for commercial purposes and should not be used as such. Feel free to study, modify, and distribute the code as you wish, always respecting the principles of the open-source community. The repository is open for contributions, and everyone is welcome to collaborate to further improve this project.
