PROG7312 Municipal Services Application

Developed by: Ethan Jason Smith (ST10263534)

⸻

Overview

The Municipal Services Application is a C# WinForms project developed as part of the PROG7312 Advanced Application Development module.
The goal is to simulate a real municipal platform allowing citizens to:
	•	Report issues
	•	View local events & announcements
	•	Track and manage service requests

The application also serves as a practical demonstration of advanced data structures and algorithms, manually implemented without using built-in List<> for primary storage.

⸻

Tech Stack
	•	Language: C#
	•	Framework: .NET WinForms
	•	IDE: Visual Studio 2022
	•	Target version: C# 7.3
	•	Storage: In-memory (custom data structures only)
	•	Data structures implemented:
	•	Custom LinkedList
	•	Custom Queue
	•	Custom Stack (via LinkedList)
	•	Custom Binary Search Tree (BST)
	•	Custom Min-Priority Queue
	•	Graph (Adjacency List) + BFS Traversal
	•	SortedDictionary
	•	HashSet
	•	Dictionary

Project Structure
/PROG7312.POE
│
├── DataStructures/
│     ├── LinkedList.cs
│     ├── Queue.cs
│     ├── BST.cs
│     ├── MinPriorityQueue.cs
│     └── Graph.cs
│
├── Models/
│     ├── Issue.cs
│     ├── EventItem.cs
│     ├── ServiceRequest.cs
│     └── Enums.cs
│
├── Services/
│     ├── IssueRepository.cs
│     ├── EventStore.cs
│     └── RequestStore.cs
│
├── UI/
│     ├── MainForm.cs
│     ├── ReportIssueForm.cs
│     ├── EventsForm.cs
│     └── RequestStatusForm.cs
│
├── Resources/
│     └── TableMountain.jpg
│
├── Program.cs
└── README.md

How to Run the Application
1.	Clone the repository
git clone (https://github.com/Onnfroy/PROG7312.POE.git)

2.	Open the solution in Visual Studio 2022
3.	Set Startup Project → PROG7312.POE
4.	Ensure target framework is .NET 4.x (as per project setup)
5.	Click Start (or press Ctrl+F5)

The main menu will appear with three navigation tiles.

Part 1 — Report Issues

✔ Issue reporting form with:
	•	Location input
	•	Category dropdown
	•	Description box
	•	File attachment viewer
	•	Progress bar
	•	Rotating confirmation messages using a custom Queue
✔ Issues stored in custom LinkedList
✔ Soft beige theme with rounded buttons and hover effects
✔ Header branding + Table Mountain image

⸻

Part 2 — Local Events & Announcements

✔ Display of 15+ events using SortedDictionary<DateTime, LinkedList<EventItem>>
✔ Category, Title, and Date search
✔ Sorting (date, category, title, etc.)
✔ Unique categories using a HashSet
✔ New event submissions managed with a custom Queue
✔ “Last viewed” using a stack-like structure (custom LinkedList)
✔ Recommendation system using a Dictionary tracking search patterns
✔ Highlighting of recommended events

⸻

Part 3 — Service Request Status

✔ Full request management screen
✔ 15+ seeded service requests
✔ Binary Search Tree (custom) for ID lookup
✔ Min-Priority Queue (custom) for scheduling next request
✔ Status workflow: Submitted → InProgress → Resolved
✔ Graph-based Route Planner using Adjacency List + BFS
✔ Partial and full ID lookup
✔ Sorting by priority, due date, category, status
✔ Completed, consistent UI theme across all forms

⸻

Data Structures Explanation

This project demonstrates the understanding and application of advanced data structures. All primary structures were implemented manually.

1. Custom LinkedList

Used for:
	•	Issue storage
	•	Event lists by date
	•	“Last viewed” stack
	•	Path representation in Graph BFS

Implements:
	•	AddLast(T)
	•	RemoveFirst()
	•	IsEmpty()
	•	Enumerator for foreach

2. Custom Queue

Used in:
	•	Engagement messages rotation
	•	New event submissions queue (Part 2)
	•	BFS in Graph traversal

Provides FIFO behaviour with:
	•	Enqueue(T)
	•	Dequeue()

3. Custom Binary Search Tree (BST)

Used to search Service Requests by ID efficiently.
Supports:
	•	Insert(key, value)
	•	TryGet(key)
	•	In-order traversal

4. Custom Min-Priority Queue

Used to determine next request to service.
Compares:
	•	Priority (Critical → Low)
	•	Due date (earliest first)

Implements:
	•	Sorted insertion
	•	PeekMin()
	•	DequeueMin()

5. Graph (Adjacency List)

Used in the Route Planner.
Supports:
	•	Adding vertices and edges
	•	BfsShortestPath(start, goal)

Shows shortest paths between depots and wards.

6. SortedDictionary & HashSet

Allowed .NET collections used to:
	•	Maintain sorted event buckets
	•	Track unique event categories
	•	Store search analytics keys

⸻

Project Completion Report (Reflection)

What went well
	•	UI design and consistency across all forms
	•	Implementation of custom data structures
	•	Separation of concerns through Models, Services, and UI folders
	•	Real-time responsiveness of filters and sorts
	•	Logical seeding and state management using AppState

Challenges & How I Overcame Them
	•	Conflicts between custom LinkedList and .NET LinkedList:
Solved by explicit namespace references.
	•	BST visual selection issues:
Fixed by forcing focus and disabling HideSelection.
	•	Keeping UI clean with many controls:
Used TableLayoutPanel structures to maintain predictable layouts.
	•	Ensuring no built-in List<> was used for primary storage:
I was careful to restrict List usage only to UI sorting, not core data.

Timeline Summary
	•	Week 1–2: Research + Part 1
	•	Week 3–4: Part 2 events module + recommendation logic
	•	Week 5–6: Part 3 service requests + graphs + priority queue
	•	Week 7: UI polishing, debugging & proofing
	•	Week 8: Documentation and final video

What I learned
	•	Deep understanding of core DS (queues, stacks, linked lists, BSTs, heaps, graphs)
	•	Applying DS structures to real UI workflows
	•	Designing consistent UI systems
	•	Managing state across multiple screens
	•	Strengthened debugging and architecture thinking

⸻

Technology Recommendations

To scale this application realistically for a municipality:

1. Move from in-memory to persistent storage
	•	Recommend SQL Server or PostgreSQL for relational data
	•	Would allow audits, multiple users, historical reports

2. Add cloud hosting
	•	Host API backend in Azure or AWS
	•	Use ASP.NET Core Web API for future-proofing
	•	Case study: City of Cape Town’s actual service portals use cloud-backed systems

3. Add mapping APIs
	•	Integrate Google Maps API or Leaflet.js for real visual routing
	•	BFS graph logic here is a good first step

4. Real user authentication
	•	Use IdentityServer or Firebase Auth

⸻

AI Usage Declaration

Portions of this project — especially debugging, refactoring, explanations, and documentation — were assisted by ChatGPT.
All final logic, design, implementation, and verification were done by me.

⸻

Demo Video

YouTube Demo Link: https://youtu.be/tgPTilh_Ys0
