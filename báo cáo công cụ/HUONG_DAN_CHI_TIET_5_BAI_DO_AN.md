# HƯỚNG DẪN CHI TIẾT 5 BÀI ĐỒ ÁN CUỐI KỲ - MUSIC PLAYER
**Áp dụng AI xuyên suốt từ Bài 1 đến Bài 5**

---

## **BÀI 1: THIẾT KẾ UI/UX VỚI FIGMA**
**Yêu cầu:** 20-30 chức năng/màn hình ✅

### **I. CÀI ĐẶT VÀ CHUẨN BỊ**
1. **Tài khoản Figma**
   - Đăng ký tài khoản Figma miễn phí
   - Upgrade lên Figma Pro nếu cần (student discount available)

2. **Plugin cần thiết**
   - Unsplash (hình ảnh minh họa)
   - Iconify (icons)
   - Content Reel (dummy data)
   - Stark (check contrast)

### **II. DESIGN SYSTEM**
**1. Color Palette**
```
Primary: #1DB954 (Spotify Green)
Secondary: #191414 (Dark Background)
Accent: #FF6B6B (Accent Red)
Text: #FFFFFF (White)
Gray: #B3B3B3 (Secondary Text)
```

**2. Typography**
```
Headings: Inter Bold (24px, 32px, 48px)
Body: Inter Regular (14px, 16px)
Buttons: Inter Semi-bold (12px, 14px)
```

**3. Components Library**
- Buttons (Primary, Secondary, Ghost)
- Input Fields
- Cards
- Navigation
- Player Controls
- Modals

### **III. 30 MÀN HÌNH BẮT BUỘC**

#### **A. AUTHENTICATION (4 MÀN HÌNH)**
1. **Login Screen**
   - Email/Password fields
   - Social login buttons
   - Forgot password link
   - Sign up link

2. **Register Screen**
   - Full name, Email, Password
   - Confirm password
   - Terms & conditions
   - Social registration

3. **Forgot Password**
   - Email input
   - Send reset button
   - Back to login

4. **Profile Setup**
   - Avatar upload
   - Username selection
   - Music preferences
   - Skip for now

#### **B. MAIN PLAYER (6 MÀN HÌNH)**
5. **Home/Now Playing**
   - Album artwork
   - Song info
   - Player controls
   - Progress bar
   - Volume control

6. **Playlist View**
   - Playlist cover
   - Song list
   - Play/Shuffle
   - Add songs button

7. **Album View**
   - Album artwork
   - Track listing
   - Release info
   - Play album button

8. **Artist View**
   - Artist image
   - Popular songs
   - Albums
   - Follow button

9. **Search Results**
   - Search bar
   - Filter tabs
   - Results grid
   - No results state

10. **Settings**
    - Profile settings
    - Audio quality
    - Theme selection
    - Logout

#### **C. USER MANAGEMENT (4 MÀN HÌNH)**
11. **User Profile**
    - Avatar and info
    - Statistics
    - Recent activity
    - Edit profile button

12. **Edit Profile**
    - Avatar upload
    - Name change
    - Email change
    - Save button

13. **Subscription/Plans**
    - Free tier features
    - Premium benefits
    - Pricing cards
    - Upgrade button

14. **Payment History**
    - Transaction list
    - Receipt download
    - Payment method
    - Billing info

#### **D. PLAYLIST MANAGEMENT (6 MÀN HÌNH)**
15. **Create Playlist**
    - Playlist name
    - Description
    - Cover image
    - Privacy settings

16. **Edit Playlist**
    - Change name/desc
    - Reorder songs
    - Remove songs
    - Add songs

17. **Share Playlist**
    - Share link
    - Social sharing
    - Collaborate option
    - Embed code

18. **Collaborative Playlist**
    - Contributors list
    - Add/remove users
    - Activity feed
    - Settings

19. **Favorite Songs**
    - Liked songs list
    - Filter options
    - Sort options
    - Remove from favorites

20. **Recently Played**
    - History list
    - Clear history
    - Play again
    - Add to playlist

#### **E. SOCIAL FEATURES (4 MÀN HÌNH)**
21. **Friends Activity**
    - Friend list
    - What they're listening to
    - Play their songs
    - Add friends

22. **Share Music**
    - Share to social
    - Copy link
    - QR code
    - Message friends

23. **Comments/Ratings**
    - Song comments
    - Star ratings
    - Review list
    - Write review

24. **Discover Weekly**
    - Personalized mix
    - Refresh button
    - Save playlist
    - Share mix

#### **F. ADDITIONAL FEATURES (6 MÀN HÌNH)**
25. **Equalizer**
    - Frequency sliders
    - Presets (Rock, Jazz, etc.)
    - Custom save
    - Reset button

26. **Sleep Timer**
    - Time selection
    - Stop playing
    - Preview end time
    - Cancel timer

27. **Offline Mode**
    - Downloaded songs
    - Storage info
    - Quality settings
    - Remove downloads

28. **Download Manager**
    - Download queue
    - Progress bars
    - Pause/resume
    - Storage management

29. **Statistics/Analytics**
    - Listening time
    - Top artists/songs
    - Genre breakdown
    - Export data

30. **Help/Support**
    - FAQ section
    - Contact support
    - Report issue
    - App version

### **IV. PROTOTYPE TƯƠNG TÁC**
**1. User Flows**
- Registration → Home → Play music
- Search → Results → Play song
- Create playlist → Add songs → Share

**2. Micro-interactions**
- Button hover states
- Loading animations
- Success/error messages
- Gesture animations

**3. Responsive Design**
- Mobile: 375x667 (iPhone SE)
- Tablet: 768x1024 (iPad)
- Desktop: 1440x900 (MacBook)

### **V. MINH HỌNG AI TRONG BÀI 1**
**AI đã hỗ trợ:**
- Generate color palette suggestions
- Create component variations
- Suggest typography combinations
- Generate dummy content
- Create responsive layouts

---

## **BÀI 2: KIỂM THỬ FRONTEND VỚI DEVELOPER TOOLS (F12)**
**Yêu cầu:** Thành thạo Chrome DevTools ✅

### **I. CÀI ĐẶT VÀ CHUẨN BỊ**
1. **Chrome Browser**
   - Latest version of Google Chrome
   - Chrome Extensions: React DevTools, Vue DevTools

2. **Music Player Application**
   - Deploy local version
   - Access via localhost:3000
   - Enable debug mode

### **II. ELEMENTS TAB TESTING**
**1. DOM Inspection**
```javascript
// Inspect player controls
document.querySelector('.player-controls')
document.querySelector('.play-button')
document.querySelector('.progress-bar')
```

**2. CSS Modification**
```css
/* Test responsive design */
.player-container {
    max-width: 100%;
    padding: 20px;
}

/* Test dark theme */
body.dark-theme {
    background-color: #191414;
    color: #FFFFFF;
}
```

**3. Responsive Testing**
- Device Toolbar: Toggle device simulation
- Test sizes: Mobile (375px), Tablet (768px), Desktop (1440px)
- Orientation: Portrait/Landscape

### **III. CONSOLE TESTING**
**1. JavaScript Debugging**
```javascript
// Test player functionality
player.play();
player.pause();
player.setVolume(50);

// Check for errors
console.log(player.getCurrentTrack());
console.log(player.getPlaylist());
```

**2. API Testing**
```javascript
// Test API calls
fetch('/api/tracks')
  .then(response => response.json())
  .then(data => console.log(data));

// Test error handling
fetch('/api/invalid-endpoint')
  .catch(error => console.error('API Error:', error));
```

### **IV. NETWORK TAB ANALYSIS**
**1. Request Monitoring**
- Filter by: XHR/Fetch, JS, CSS, Img
- Check status codes: 200, 404, 500
- Response times: <200ms ideal

**2. Performance Testing**
- Waterfall view
- Resource loading order
- Cache strategies

### **V. SOURCES TAB DEBUGGING**
**1. Breakpoint Debugging**
```javascript
// Set breakpoints in player.js
function playTrack(trackId) {
    debugger; // Breakpoint here
    // Play logic
}
```

**2. Call Stack Analysis**
- Trace function calls
- Identify bottlenecks
- Memory leak detection

### **VI. PERFORMANCE TAB**
**1. Performance Recording**
- Record interactions
- Analyze FPS
- Identify jank

**2. Memory Analysis**
- Heap snapshots
- Memory leaks
- Garbage collection

### **VII. MINH HỌNG AI TRONG BÀI 2**
**AI đã hỗ trợ:**
- Generate test scripts
- Debug JavaScript errors
- Suggest performance optimizations
- Create responsive test cases
- Analyze network patterns

---

## **BÀI 3: QUẢN LÝ MÃ NGUỒN VỚI GITHUB**
**Yêu cầu:** Workflow chuẩn với branching strategy ✅

### **I. CẤU TRÚC REPOSITORY**
```
MusicProject_Final/
├── .github/
│   ├── workflows/
│   │   ├── ci-cd.yml
│   │   └── code-quality.yml
│   ├── ISSUE_TEMPLATE/
│   └── PULL_REQUEST_TEMPLATE.md
├── docs/
├── src/
├── tests/
├── .gitignore
├── README.md
└── package.json
```

### **II. BRANCHING STRATEGY**
**1. Main Branches**
```
main (production)
├── develop (staging)
├── feature/ui-design
├── feature/frontend-testing
├── feature/backend-api
├── feature/authentication
├── feature/payment-integration
└── hotfix/critical-bug
```

**2. Branch Naming Convention**
```
feature/task-description
bugfix/bug-description
hotfix/urgent-fix
release/version-number
```

### **III. WORKFLOW CHUẨN**
**1. Feature Development**
```bash
# 1. Create feature branch
git checkout develop
git pull origin develop
git checkout -b feature/music-player-ui

# 2. Development
# ... code changes ...

# 3. Commit changes
git add .
git commit -m "feat: implement music player UI components"

# 4. Push to remote
git push origin feature/music-player-ui

# 5. Create Pull Request
# GitHub UI → New Pull Request
```

**2. Commit Message Convention**
```
feat: new feature
fix: bug fix
docs: documentation
style: formatting
refactor: code refactoring
test: adding tests
chore: maintenance
```

### **IV. PULL REQUEST TEMPLATE**
```markdown
## Description
Brief description of changes

## Type of Change
- [ ] Bug fix
- [ ] New feature
- [ ] Breaking change
- [ ] Documentation update

## Testing
- [ ] Unit tests pass
- [ ] Integration tests pass
- [ ] Manual testing completed

## Screenshots
[Add screenshots if applicable]

## Checklist
- [ ] Code follows style guidelines
- [ ] Self-review completed
- [ ] Documentation updated
```

### **V. CODE REVIEW PROCESS**
**1. Review Checklist**
- Code quality and style
- Functionality correctness
- Performance implications
- Security considerations
- Test coverage

**2. Reviewers Assignment**
- At least 2 reviewers required
- Team lead approval for main branch
- Automated checks must pass

### **VI. CI/CD PIPELINE**
**1. GitHub Actions Workflow**
```yaml
name: CI/CD Pipeline

on:
  push:
    branches: [main, develop]
  pull_request:
    branches: [main, develop]

jobs:
  test:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v2
      - name: Setup Node.js
        uses: actions/setup-node@v2
        with:
          node-version: '18'
      - name: Install dependencies
        run: npm ci
      - name: Run tests
        run: npm test
      - name: Run linting
        run: npm run lint

  deploy:
    needs: test
    runs-on: ubuntu-latest
    if: github.ref == 'refs/heads/main'
    steps:
      - name: Deploy to production
        run: echo "Deploy to production"
```

### **VII. MINH HỌNG AI TRONG BÀI 3**
**AI đã hỗ trợ:**
- Generate commit message templates
- Create GitHub Actions workflows
- Suggest branching strategy
- Generate PR templates
- Code review automation

---

## **BÀI 4: QUẢN TRỊ DỰ ÁN VỚI JIRA**
**Yêu cầu:** 10 Epics, 20 Stories, 30-40 Tasks ✅

### **I. PROJECT SETUP**
**1. Jira Project Configuration**
- Project Type: Software Development
- Project Key: MUSIC
- Workflow: Kanban/Scrum
- Dashboard: Music Player Development

**2. Components Structure**
```
Frontend (UI/UX)
Backend (API)
Database (Schema)
Testing (QA)
DevOps (Deployment)
Documentation
Mobile App
Web App
API Integration
Performance
```

### **II. EPICS (10 EPICS)**
**1. MUSIC-1: User Authentication System**
**2. MUSIC-2: Music Player Core Features**
**3. MUSIC-3: Playlist Management**
**4. MUSIC-4: Search and Discovery**
**5. MUSIC-5: Social Features**
**6. MUSIC-6: Payment Integration**
**7. MUSIC-7: Admin Dashboard**
**8. MUSIC-8: Mobile Application**
**9. MUSIC-9: Performance Optimization**
**10. MUSIC-10: Analytics and Reporting**

### **III. USER STORIES (20 STORIES)**

#### **Epic 1: User Authentication**
**MUSIC-11:** As a user, I want to register with email/password
**MUSIC-12:** As a user, I want to login with social media
**MUSIC-13:** As a user, I want to reset my forgotten password
**MUSIC-14:** As a user, I want to enable two-factor authentication

#### **Epic 2: Music Player Core**
**MUSIC-15:** As a user, I want to play/pause music
**MUSIC-16:** As a user, I want to control volume
**MUSIC-17:** As a user, I want to seek to specific time
**MUSIC-18:** As a user, I want to shuffle/repeat songs

#### **Epic 3: Playlist Management**
**MUSIC-19:** As a user, I want to create playlists
**MUSIC-20:** As a user, I want to add songs to playlists
**MUSIC-21:** As a user, I want to share playlists
**MUSIC-22:** As a user, I want to collaborate on playlists

#### **Epic 4: Search and Discovery**
**MUSIC-23:** As a user, I want to search for songs
**MUSIC-24:** As a user, I want to browse by genre
**MUSIC-25:** As a user, I want to discover new music
**MUSIC-26:** As a user, I want to see recommendations

#### **Epic 5: Social Features**
**MUSIC-27:** As a user, I want to follow other users
**MUSIC-28:** As a user, I want to see friends' activity
**MUSIC-29:** As a user, I want to share what I'm listening
**MUSIC-30:** As a user, I want to comment on songs

### **IV. TASKS (35 TASKS)**

#### **Frontend Tasks**
**MUSIC-101:** Design login screen UI
**MUSIC-102:** Implement music player controls
**MUSIC-103:** Create playlist interface
**MUSIC-104:** Build search functionality
**MUSIC-105:** Design responsive layout
**MUSIC-106:** Implement dark mode
**MUSIC-107:** Add loading states
**MUSIC-108:** Create error handling UI

#### **Backend Tasks**
**MUSIC-201:** Setup authentication API
**MUSIC-202:** Implement music streaming
**MUSIC-203:** Create playlist endpoints
**MUSIC-204:** Build search API
**MUSIC-205:** Implement user management
**MUSIC-206:** Add file upload functionality
**MUSIC-207:** Create notification system
**MUSIC-208:** Implement caching strategy

#### **Database Tasks**
**MUSIC-301:** Design user schema
**MUSIC-302:** Create music metadata tables
**MUSIC-303:** Implement playlist relationships
**MUSIC-304:** Setup search indexing
**MUSIC-305:** Create audit logging
**MUSIC-306:** Implement data migration
**MUSIC-307:** Optimize queries
**MUSIC-308:** Setup backup strategy

#### **Testing Tasks**
**MUSIC-401:** Write unit tests for auth
**MUSIC-402:** Create integration tests
**MUSIC-403:** Implement E2E tests
**MUSIC-404:** Performance testing
**MUSIC-405:** Security testing
**MUSIC-406:** Accessibility testing
**MUSIC-407:** Mobile testing
**MUSIC-408:** Cross-browser testing

#### **DevOps Tasks**
**MUSIC-501:** Setup CI/CD pipeline
**MUSIC-502:** Configure monitoring
**MUSIC-503:** Implement logging
**MUSIC-504:** Setup backup systems
**MUSIC-505:** Configure security scanning
**MUSIC-506:** Implement auto-scaling
**MUSIC-507:** Setup CDN
**MUSIC-508:** Configure SSL certificates

### **V. WORKFLOW CONFIGURATION**
**1. Board Columns**
```
Backlog → To Do → In Progress → Code Review → Testing → Done
```

**2. Issue Types**
- Epic (Large features)
- Story (User functionality)
- Task (Technical work)
- Bug (Issues to fix)
- Sub-task (Breakdown work)

### **VI. DASHBOARD SETUP**
**1. Widgets**
- Sprint Burndown
- Velocity Chart
- Cumulative Flow Diagram
- Issue Statistics
- Recent Activity
- Project Health

### **VII. INTEGRATION WITH GITHUB**
**1. GitHub Integration**
- Connect Jira to GitHub
- Auto-create branches from issues
- Link commits to issues
- Sync PR status

### **VIII. BUG TRACKING**
**1. Bug Report Template**
```markdown
**Summary:** Brief description
**Steps to Reproduce:**
1. Step 1
2. Step 2
**Expected Result:** What should happen
**Actual Result:** What actually happened
**Environment:** OS, Browser, Version
**Attachments:** Screenshots, Logs
```

**2. Bug Priority Levels**
- Blocker: System unusable
- Critical: Major functionality broken
- Major: Significant impact
- Minor: Small issues
- Trivial: Cosmetic issues

### **IX. MINH HỌNG AI TRONG BÀI 4**
**AI đã hỗ trợ:**
- Generate Epic and Story templates
- Create task breakdown structures
- Suggest workflow configurations
- Generate dashboard widgets
- Create bug report templates

---

## **BÀI 5: TRIỂN KHAI TRÊN CLOUD**
**Yêu cầu:** Azure + Docker + CI/CD ✅

### **I. PLATFORM SELECTION: MICROSOFT AZURE**
**1. Azure Services Architecture**
```
Azure App Service (Web App)
├── Azure SQL Database
├── Azure Blob Storage (Music files)
├── Azure CDN (Content delivery)
├── Azure Active Directory (Auth)
├── Azure DevOps (CI/CD)
├── Application Insights (Monitoring)
└── Azure Key Vault (Secrets)
```

### **II. DOCKER CONTAINERIZATION**
**1. Dockerfile for Music Player**
```dockerfile
# Multi-stage build
FROM node:18-alpine AS builder
WORKDIR /app
COPY package*.json ./
RUN npm ci --only=production
COPY . .
RUN npm run build

# Production stage
FROM nginx:alpine
COPY --from=builder /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

**2. Docker Compose**
```yaml
version: '3.8'
services:
  music-app:
    build: .
    ports:
      - "80:80"
    environment:
      - NODE_ENV=production
    depends_on:
      - database
      - redis

  database:
    image: mcr.microsoft.com/mssql/server:2019-latest
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourPassword123
    ports:
      - "1433:1433"

  redis:
    image: redis:alpine
    ports:
      - "6379:6379"
```

### **III. AZURE DEPLOYMENT SETUP**
**1. Azure App Service Configuration**
```bash
# Create resource group
az group create --name MusicPlayerRG --location eastus

# Create app service plan
az appservice plan create --name MusicPlayerPlan --resource-group MusicPlayerRG --sku B1

# Create web app
az webapp create --name MusicPlayerApp --resource-group MusicPlayerRG --plan MusicPlayerPlan
```

**2. Azure SQL Database**
```bash
# Create SQL server
az sql server create --name musicplayerserver --resource-group MusicPlayerRG --admin-user adminuser --admin-password YourPassword123

# Create database
az sql db create --name MusicPlayerDB --resource-group MusicPlayerRG --server musicplayerserver
```

**3. Azure Storage Account**
```bash
# Create storage account
az storage account create --name musicplayerstorage --resource-group MusicPlayerRG --sku Standard_LRS

# Create blob container
az storage container create --name musicfiles --account-name musicplayerstorage
```

### **IV. CI/CD PIPELINE WITH AZURE DEVOPS**
**1. Azure Pipelines YAML**
```yaml
trigger:
- main

pool:
  vmImage: 'ubuntu-latest'

variables:
  azureSubscription: 'AzureConnection'
  appName: 'MusicPlayerApp'
  resourceGroupName: 'MusicPlayerRG'

stages:
- stage: Build
  displayName: 'Build and Test'
  jobs:
  - job: Build
    displayName: 'Build Job'
    steps:
    - task: NodeTool@0
      inputs:
        versionSpec: '18.x'
      displayName: 'Install Node.js'
    
    - script: npm ci
      displayName: 'Install dependencies'
    
    - script: npm test
      displayName: 'Run tests'
    
    - script: npm run build
      displayName: 'Build application'

- stage: Deploy
  displayName: 'Deploy to Azure'
  dependsOn: Build
  jobs:
  - job: Deploy
    displayName: 'Deploy Job'
    steps:
    - task: AzureWebApp@1
      inputs:
        azureSubscription: $(azureSubscription)
        appName: $(appName)
        package: '$(System.DefaultWorkingDirectory)/**/*.zip'
```

### **V. MONITORING AND LOGGING**
**1. Application Insights**
```javascript
// Add to application
import { ApplicationInsights } from '@microsoft/applicationinsights-web';

const appInsights = new ApplicationInsights({
  config: {
    instrumentationKey: 'YOUR-INSTRUMENTATION-KEY',
    enableAutoRouteTracking: true
  }
});

appInsights.loadAppInsights();
```

**2. Log Analytics**
```bash
# Setup log analytics workspace
az monitor log-analytics workspace create --name MusicPlayerLogs --resource-group MusicPlayerRG
```

### **VI. SECURITY CONFIGURATION**
**1. Azure Active Directory**
```bash
# Create AAD app
az ad app create --display-name "Music Player" --sign-in-audience AzureADMyOrg
```

**2. Key Vault for Secrets**
```bash
# Create key vault
az keyvault create --name MusicPlayerVault --resource-group MusicPlayerRG

# Add secrets
az keyvault secret set --vault-name MusicPlayerVault --name "DatabaseConnection" --value "your-connection-string"
```

### **VII. PERFORMANCE OPTIMIZATION**
**1. Azure CDN Setup**
```bash
# Create CDN profile
az cdn profile create --name MusicPlayerCDN --resource-group MusicPlayerRG --sku Standard

# Create CDN endpoint
az cdn endpoint create --name musicplayer --profile-name MusicPlayerCDN --resource-group MusicPlayerRG --origin musicplayerapp.azurewebsites.net
```

**2. Auto-scaling Configuration**
```bash
# Create auto-scale rule
az monitor autoscale create --resource-group MusicPlayerRG --resource MusicPlayerPlan --resource-type Microsoft.Web/serverfarms --min-count 1 --max-count 5 --count 1
```

### **VIII. BACKUP AND DISASTER RECOVERY**
**1. Database Backup**
```bash
# Configure automated backups
az sql db backup-policy show --resource-group MusicPlayerRG --server musicplayerserver --name MusicPlayerDB
```

**2. Application Backup**
```bash
# Create backup schedule
az webapp config backup create --resource-group MusicPlayerRG --webapp-name MusicPlayerApp --backup-name MusicPlayerBackup --storage-account-url https://musicplayerstorage.blob.core.windows.net/
```

### **IX. ENVIRONMENT CONFIGURATION**
**1. Development Environment**
```yaml
# docker-compose.dev.yml
version: '3.8'
services:
  music-app:
    build:
      context: .
      dockerfile: Dockerfile.dev
    volumes:
      - .:/app
      - /app/node_modules
    ports:
      - "3000:3000"
    environment:
      - NODE_ENV=development
      - CHOKIDAR_USEPOLLING=true
```

**2. Production Environment**
```yaml
# docker-compose.prod.yml
version: '3.8'
services:
  music-app:
    image: musicplayer:latest
    ports:
      - "80:80"
    environment:
      - NODE_ENV=production
    restart: unless-stopped
```

### **X. DEPLOYMENT SCRIPTS**
**1. Deploy Script**
```bash
#!/bin/bash
# deploy.sh

echo "Building Docker image..."
docker build -t musicplayer:latest .

echo "Pushing to Azure Container Registry..."
az acr login --name MusicPlayerRegistry
docker tag musicplayer:latest MusicPlayerRegistry.azurecr.io/musicplayer:latest
docker push MusicPlayerRegistry.azurecr.io/musicplayer:latest

echo "Deploying to Azure App Service..."
az webapp config container set --resource-group MusicPlayerRG --name MusicPlayerApp --docker-custom-image-name MusicPlayerRegistry.azurecr.io/musicplayer:latest

echo "Deployment completed!"
```

### **XI. MINH HỌNG AI TRONG BÀI 5**
**AI đã hỗ trợ:**
- Generate Docker configurations
- Create Azure deployment scripts
- Design CI/CD pipeline
- Suggest security configurations
- Optimize performance settings

---

## **TỔNG KẾT VÀ HƯỚNG DẪN BÁO CÁO**

### **I. CẤU TRÚC BÁO CÁO CUỐI KỲ (25-40 TRANG)**
```
1. Trang bìa (1 trang)
2. Mục lục (1 trang)
3. Giới thiệu dự án (2-3 trang)
4. Bài 1: UI/UX Design (5-6 trang)
5. Bài 2: Frontend Testing (4-5 trang)
6. Bài 3: GitHub Workflow (4-5 trang)
7. Bài 4: Jira Management (5-6 trang)
8. Bài 5: Cloud Deployment (5-6 trang)
9. Kết quả và Đánh giá (2-3 trang)
10. Kết luận (1-2 trang)
```

### **II. NỘI DUNG BẮT BUỘC**
1. **Hình ảnh minh chứng** cho mỗi bài
2. **Ghi chú rõ vai trò AI** trong từng phần
3. **Code samples và configurations**
4. **Screenshots và diagrams**
5. **Kết quả thực tế** của việc triển khai

### **III. LƯU Ý QUAN TRỌNG**
- Mỗi thành viên phải tham gia tất cả các hạng mục
- Ghi rõ AI đã hỗ trợ ở đâu và như thế nào
- Cung cấp đầy đủ bằng chứng minh họa
- Báo cáo phải có tính thực tế cao

### **IV. THỜI GIAN THỰC HIỆN ĐỀ XUẤT**
- Tuần 1-2: Bài 1 (UI/UX Design)
- Tuần 3: Bài 2 (Frontend Testing)
- Tuần 4: Bài 3 (GitHub Workflow)
- Tuần 5: Bài 4 (Jira Management)
- Tuần 6: Bài 5 (Cloud Deployment)
- Tuần 7-8: Hoàn thiện báo cáo

---

**Chúc bạn hoàn thành thành công đồ án cuối kỳ! 🎵**

**Lưu ý:** Hãy thực hiện tuần tự từng bài và chụp lại minh chứng cho mỗi bước để đưa vào báo cáo cuối kỳ.
