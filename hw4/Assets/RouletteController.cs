using UnityEngine;
using UnityEngine.InputSystem;

public class RouletteController : MonoBehaviour
{
    [Header("時間設定")]
    public float minSpinDuration = 5.0f; // 確保至少轉 5 秒
    public float maxSpinDuration = 8.0f; // 最長轉 8 秒

    [Header("速度設定")]
    public float initialSpeed = 1000f;   // 初始旋轉速度
    public float friction = 0.96f;      // 進入減速階段後的摩擦力

    private float currentSpeed = 0f;
    private float timer = 0f;
    private float targetDuration = 0f;
    private bool isSpinning = false;
    private bool isSlowingDown = false;

    void Update()
    {
        // 1. 偵測 Enter 鍵啟動
        bool enterPressed = Keyboard.current != null &&
                           (Keyboard.current.enterKey.wasPressedThisFrame ||
                            Keyboard.current.numpadEnterKey.wasPressedThisFrame);

        if (enterPressed && !isSpinning)
        {
            StartSpin();
        }

        if (isSpinning)
        {
            // 執行旋轉
            transform.Rotate(0, 0, currentSpeed * Time.deltaTime);

            if (!isSlowingDown)
            {
                // 階段 A：固定速度旋轉計時
                timer += Time.deltaTime;
                if (timer >= targetDuration)
                {
                    isSlowingDown = true; // 時間到了，進入減速階段
                }
            }
            else
            {
                // 階段 B：慢慢停下來
                currentSpeed *= friction;

                if (currentSpeed < 0.5f)
                {
                    StopSpin();
                }
            }
        }
    }

    void StartSpin()
    {
        timer = 0f;
        targetDuration = Random.Range(minSpinDuration, maxSpinDuration); // 隨機決定這次要轉幾秒
        currentSpeed = initialSpeed;
        isSpinning = true;
        isSlowingDown = false;
        Debug.Log($"開始旋轉！預計定速旋轉 {targetDuration:F1} 秒後開始減速");
    }

    void StopSpin()
    {
        currentSpeed = 0f;
        isSpinning = false;
        isSlowingDown = false;
        Debug.Log("輪盤停止");
    }
}