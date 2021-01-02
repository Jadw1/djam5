using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTrail : MonoBehaviour
{
    public TrailRenderer trailRenderer;
    
    public void StartTrail(float duration, List<Vector3> points)
    {
        duration = duration / 4.0f;
        trailRenderer.time = duration;
        StartCoroutine(Move(points, duration));
    }

    IEnumerator Move(List<Vector3> points, float duration)
    {
        var index = 1;
        var progress = 0.0f;
        var startPos = transform.position;
        var targetPos = points[index] + Vector3.up;
        var distance = Vector3.Distance(startPos, targetPos);

        while (true)
        {
            var durationPerSegment = duration / points.Count;
            progress += Time.deltaTime * (1 / (distance * durationPerSegment));
            transform.position = Vector3.Lerp(startPos, targetPos, progress);

            if (progress >= 1.0f)
            {
                progress = 0.0f;
                index += 1;
                if (index >= points.Count)
                {
                    yield break;
                }
                
                startPos = transform.position;
                targetPos = points[index] + Vector3.up;
            }
            
            yield return null;
        }
    }
}
