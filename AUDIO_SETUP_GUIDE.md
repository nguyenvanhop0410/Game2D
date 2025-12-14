# H??NG D?N CÀI ??T H? TH?NG ÂM THANH

## ?? T?ng Quan
Hệ thống âm thanh đã được thêm vào game với các tính năng:
- ? Nhạc nền (Menu, Gameplay, Win, Lose)
- ? Hiệu ứng âm thanh (SFX) cho Player, Chest, UI
- ? Quản lý âm lượng riêng cho Music và SFX
  - ? Singleton pattern - không mất âm thanh khi chuyển scene

## ?? Các Script Đã Tạo

### 1. AudioManager.cs
Script quản lý toàn bộ âm thanh trong game.
- **Vị trí**: Assets/Scripts/AudioManager.cs
- **Chức năng**: Singleton, quản lý music + SFX

### 2. ButtonSoundEffect.cs
Component tự động phát âm thanh cho Button.
- **Vị trí**: Assets/Scripts/ButtonSoundEffect.cs
- **Cách dùng**: Attach vào bất kỳ Button nào trong game

## ??? CÁCH CÀI ĐẶT

### Bước 1: Tạo GameObject AudioManager trong Scene Menu

1. Mở scene **MainMenu** (hoặc scene khởi đầu của bạn)
2. Tạo GameObject mới:
   - Chuột phải trong Hierarchy -> Create Empty
   - Đặt tên: **AudioManager**
3. Add Component AudioManager:
   - Chọn AudioManager object -> Add Component -> tìm "AudioManager"

### Bước 2: Chuẩn Bị File Âm Thanh

Tạo thư mục: `Assets/Audio/`

Cấu trúc khuyến nghị:               
```
Assets/
??? Audio/
    ??? Music/
    ?   ??? MenuMusic.mp3
    ?   ??? GameplayMusic.mp3
    ?   ??? WinMusic.mp3
    ?   ??? LoseMusic.mp3
    ??? SFX/
        ??? Player/
        ?   ??? Walk.wav
        ?   ??? Damage.wav
        ?   ??? Heal.wav
        ??? Chest/
        ?   ??? ChestOpen.wav
        ?   ??? ChestCollect.wav
        ?   ??? Trap.wav
        ??? UI/
            ??? ButtonClick.wav
            ??? CorrectAnswer.wav
            ??? WrongAnswer.wav
            ??? QuestPopup.wav
```

**Nguồn âm thanh miễn phí:**
    - ?? Nhạc nền: https://freepd.com/ hoặc https://incompetech.com/
    - ?? SFX: https://freesound.org/ hoặc https://mixkit.co/free-sound-effects/

### Bước 3: Gán Audio Clips vào AudioManager

Chọn AudioManager object trong Hierarchy, trong Inspector:

#### ?? Audio Sources
- **Music Source**: ?? trống (tự động tạo)
- **SFX Source**: ?? trống (tự động tạo)

#### ?? Background Music
- **Menu Music**: Kéo file nhạc menu vào đây
- **Gameplay Music**: Kéo file nhạc gameplay vào đây
- **Win Music**: Kéo file nhạc chiến thắng vào đây
- **Lose Music  **: Kéo file nhạc thua cuộc vào đây

#### ?? Sound Effects - Player
- **Player Walk Sound**: Kéo file âm thanh bước chân
- **Player Damage Sound**: Kéo file âm thanh nhận sát thương
- **Player Heal Sound**: Kéo file âm thanh hồi máu

#### ?? Sound Effects - Chest
- **Chest Open Sound**: Kéo file âm thanh mở rương
- **Chest Collect Sound**: Kéo file âm thanh thu thập rương
- **Trap Sound**: Kéo file âm thanh kích trap

#### ?? Sound Effects - UI
- **Button Click Sound**: Kéo file âm thanh click nút
- **Correct Answer Sound**: Kéo file âm thanh trả lời đúng
- **Wrong Answer Sound**: Kéo file âm thanh trả lời sai
- **Quest Popup Sound**: Kéo file âm thanh hiện câu hỏi

#### ?? Volume Settings
- **Music Volume**: 0.7 (70%)
- **SFX Volume**: 1.0 (100%)

### Bước 4: Thêm AudioManager vào Scene Gameplay

**Không cần làm gì!** AudioManager sẽ dùng `DontDestroyOnLoad()`, nó sẽ tự động tồn tại khi chuyển scene.

### Bước 5: (Tùy Chọn) Thêm ButtonSoundEffect vào các Button

?? Button tự động phát âm thanh khi click:

1. Chọn một Button trong Hierarchy
2. Add Component -> tìm "ButtonSoundEffect"
3. Xong! Button sẽ tự động phát âm thanh

**Lưu ý**: MainMenu đã được tích hợp sẵn âm thanh trong code, không cần ButtonSoundEffect.

## ?? CÁCH SỬ DỤNG TRONG CODE

### Phát nhạc nền
```csharp
// Phát nhạc menu
AudioManager.Instance.PlayMenuMusic();

// Phát nhạc gameplay
AudioManager.Instance.PlayGameplayMusic();

// Phát nhạc chiến thắng
AudioManager.Instance.PlayWinMusic();

// Phát nhạc thua cuộc
AudioManager.Instance.PlayLoseMusic();
```

### Phát hiệu ứng âm thanh
```csharp
// Âm thanh người chơi
AudioManager.Instance.PlayPlayerWalk();
AudioManager.Instance.PlayPlayerDamage();
AudioManager.Instance.PlayPlayerHeal();

// Âm thanh rương
AudioManager.Instance.PlayChestOpen();
AudioManager.Instance.PlayChestCollect();
AudioManager.Instance.PlayTrap();

// Âm thanh UI
AudioManager.Instance.PlayButtonClick();
AudioManager.Instance.PlayCorrectAnswer();
AudioManager.Instance.PlayWrongAnswer();
AudioManager.Instance.PlayQuestPopup();
```

### Điều khiển âm lượng
```csharp
// Đặt âm lượng nhạc (0.0 - 1.0)
AudioManager.Instance.SetMusicVolume(0.5f);

// Đặt âm lượng SFX (0.0 - 1.0)
AudioManager.Instance.SetSFXVolume(0.8f);

// Tắt/bật tất cả âm thanh
AudioManager.Instance.MuteAll();
AudioManager.Instance.UnmuteAll();
AudioManager.Instance.ToggleMute();
```

## ? CÁC TÍNH NĂNG ĐÃ TÍCH HỢP

### Player.cs
- ? Âm thanh bước chân khi di chuyển
- ? Âm thanh nhận sát thương
- ? Âm thanh hồi năng lượng
- ? Âm thanh kích trap
- ? Nhạc win/lose khi kết thúc

### ChestQuestion.cs
- ? Âm thanh mở rương
- ? Âm thanh thu thập rương khi đúng

### QuestionUI.cs
- ? Âm thanh chọn đáp án đúng
    - ? Âm thanh chọn đáp án sai
- ? Âm thanh hiện popup câu hỏi

### MainMenu.cs
- ? Âm thanh click button
- ? Chuyển nhạc khi vào game

## ?? KHUYẾN NGHỊ ÂM THANH

### Nhạc Nền (Music)
- **Format  **: MP3 hoặc OGG (nén tốt)
- **Import Settings trong Unity**:
  - Load Type: **Streaming**
  - Compression Format: **Vorbis**
  - Quality: 70-100%
  - Preload Audio Data: **True**

### Hiệu Ứng Âm Thanh (SFX)
- **Format**: WAV (chất lượng cao, phản hồi nhanh)
- **Import Settings trong Unity**:
  - Load Type: **Decompress On Load**
  - Compression Format: **PCM**
  - Preload Audio Data: **True**

### ?? Dài Khuyến Nghị
- Âm thanh nút bấm: 0.1-0.3 giây
- Âm thanh bước chân: 0.2-0.5 giây
- Âm thanh hiệu ứng: 0.5-2 giây
  - Nhạc nền: 1-3 phút (loop)

## ?? XỬ LÝ LỖI

### Lỗi: "AudioManager.Instance is null"
**Nguyên nhân**: Chưa tạo AudioManager object trong scene ưu tiên.
**Giải pháp**: Tạo GameObject AudioManager và add script AudioManager trong scene Menu.

### Lỗi: Không có âm thanh
**Kiểm tra**:
1. AudioManager object có được khởi tạo không?  
3. Âm lượng Music/SFX có > 0 không?
4. AudioListener có trong scene không? (thường gắn vào Main Camera)
### Âm thanh bước chân phát quá nhiều
**Giải pháp**: Trong AudioManager.PlayPlayerWalk(), đã có kiểm tra `!sfxSource.isPlaying` để tránh spam.a        `!sfxSource.isPlaying` ?? tránh spam.
## ?? MỞ RỘNGNG
### Thêm âm thanh mới
1. Import file audio vào thư mục Assets/Audio/
2. Mở AudioManager.cs
3. Thêm field SerializeField cho AudioClip mới
4. Thêm method public để phát âm thanh đóp m?i
4. Thêm method public ?? phát âm thanh ?ó
5. Gán AudioClip trong Inspector
### Ví dụ thêm âm thanh nhảy:y:
```csharp
// Trong AudioManager.cs
[Header("Sound Effects - Player")]
[SerializeField] private AudioClip playerJumpSound; // Thêm dòng này

// Thêm method
public void PlayPlayerJump()
{
    PlaySFX(playerJumpSound);
}
```
## ?? HỖ TRỢ
Nếu gặp vấn đề gì, kiểm tra Console trong Unity để xem log chi tiết.t.
**Chúc bạn thành công! ????****
